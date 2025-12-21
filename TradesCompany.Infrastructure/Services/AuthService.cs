using Microsoft.AspNetCore.Identity;
using TradesCompany.Application.Common;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRepository<ServiceMan> _serviceManRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IRepository<ServiceMan> serviceManRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _serviceManRepository = serviceManRepository;
        }

        public async Task<ServiceResult> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ServiceResult.Failure(new[] { "Invalid login attempt." });
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return ServiceResult.Success();
            }

            return ServiceResult.Failure(new[] { "Invalid login attempt." });
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ServiceResult> RegisterCustomerAsync(RegisterDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return ServiceResult.Failure(new[] { "A user with this email already exists." });
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return ServiceResult.Failure(new[] { "Invalid role specified." });
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email.ToLowerInvariant(),
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!roleResult.Succeeded)
                {
                    // Rollback? Hard with Identity. Log error usually.
                    return ServiceResult.Failure(roleResult.Errors.Select(e => e.Description));
                }
                return ServiceResult.Success();
            }

            return ServiceResult.Failure(result.Errors.Select(e => e.Description));
        }

        public async Task<ServiceResult> RegisterEmployeeAsync(EmployeeRegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);

                var serviceman = new ServiceMan
                {
                    UserId = user.Id,
                    ServiceTypeId = model.ServiceTypeId,
                };
                await _serviceManRepository.InsertAsync(serviceman);
                await _serviceManRepository.SaveAsync();
                
                // Auto sign-in? Usually yes for registration. Let's assume controller handles SignIn or we do it here. 
                // Interface says Register, usually doesn't imply AutoLogin, but AccountController did.
                // Let's keep it pure Register. Controller can call Login or SignInManager.SignInAsync(user). 
                // For simplicity, let's leave SignIn to Controller or add AutoLogin here? 
                // SignInManager is here. 
                // Let's add SignIn here? No, LoginAsync is separate. 
                // The Controller does: Register -> Redirect to Login OR Register -> SignIn.
                // AccountController CustomerRegister does: Redirect to Login (Lines 125 commented out).
                // AccountController EmployeeRegister does: Redirect to Dashboard (implies SignIn?).
                // Wait, EmployeeRegister code (Line 182) redirects to Dashboard but NEVER calls SignInAsync! This works only if user is already signed in? OR if CreateAsync signs in? (No it doesn't).
                // Ah, line 124 is commented out.
                // Wait, if EmployeeRegister doesn't Sign In, how can they access "Dashboard" [Authorize]?
                // Maybe they log in manually?
                // Line 222 in Login is PasswordSignInAsync.
                // Let's assume user needs to login. OR I should check if `EmployeeRegister` was auto-logging in.
                
                // Retrying analysis of `AccountController.EmployeeRegister` (Line 159):
                // It creates user, adds role, inserts serviceman. Returns RedirectToAction("Dashboard").
                // It does NOT call `SignInAsync`.
                // So the user is NOT authenticated after registration. 
                // Redirecting to Dashboard [Authorize] will bounce them to Login.
                // This seems like a bug or intended flow (Register -> Login).
                // CustomerRegister (Line 73) also redirects to Login.
                // So consistent: Register -> Login.
                
                return ServiceResult.Success();
            }

            return ServiceResult.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
