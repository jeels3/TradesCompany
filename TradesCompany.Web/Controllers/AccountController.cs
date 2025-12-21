using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Web.ViewModel;

namespace TradesCompany.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<ApplicationUser> _signInManager; // Kept for External Login
        private readonly UserManager<ApplicationUser> _userManager; // Kept for External Login
        private readonly IRepository<ServiceType> _serviceTypeGRepository;
        private readonly ILogger<AccountController> _logger;
        private readonly IRepository<ServiceMan> _serviceManGRepository;


        public AccountController(
            IAuthService authService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IRepository<ServiceType> serviceTypeGRepository,
            ILogger<AccountController> logger,
            IRepository<ServiceMan> serviceManGRepository)
        {
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;
            _serviceTypeGRepository = serviceTypeGRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceManGRepository = serviceManGRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CustomerRegister()
        {
            RegisterViewModel model = new RegisterViewModel
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EmployeeRegister()
        {
            var serviceTypes = await _serviceTypeGRepository.GetAllAsync();
            if (serviceTypes == null)
            {
                _logger.LogWarning("No service types found for employee registration.");
                ModelState.AddModelError(string.Empty, "No service types available for registration.");
                return View(new EmployeeRegisterViewModel());
            }
            EmployeeRegisterViewModel model = new EmployeeRegisterViewModel
            {
                ServiceTypes = (List<ServiceType>)serviceTypes,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerRegister(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())?.ToList()
                        ?? new List<AuthenticationScheme>();
                    return View(model);
                }

                var registerDto = new TradesCompany.Application.DTOs.RegisterDto
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = model.Password,
                    Role = model.Role
                };

                var result = await _authService.RegisterCustomerAsync(registerDto);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} created successfully", model.Email);
                    TempData["SuccessMessage"] = "Registration successful!";
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during customer registration for email: {Email}", model?.Email);
                ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
            }

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())?.ToList()
                ?? new List<AuthenticationScheme>();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EmployeeRegister(EmployeeRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerDto = new TradesCompany.Application.DTOs.EmployeeRegisterDto
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = model.Password,
                    Role = model.Role,
                    ServiceTypeId = model.ServiceTypeId
                };

                var result = await _authService.RegisterEmployeeAsync(registerDto);

                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard", "Employee");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegistrationError", error);
                }
            }
            var serviceTypes = await _serviceTypeGRepository.GetAllAsync();
            model.ServiceTypes = (List<ServiceType>?)serviceTypes;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? ReturnUrl = null)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginDto = new TradesCompany.Application.DTOs.LoginDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                };

                var result = await _authService.LoginAsync(loginDto);

                if (result.Succeeded)
                {
                    // Fetch user to get roles for redirection (Logic partially in Controller still, to keep simple redirection)
                    // Ideally AuthService returns Role or we decode User claim here since cookie is set.
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("EMPLOYEE"))
                    {
                        if (model.ReturnUrl != null) return Redirect(model.ReturnUrl);
                        return RedirectToAction("Dashboard", "Employee");
                    }
                    else if (roles.Contains("USER"))
                    {
                        if (model.ReturnUrl != null) return Redirect(model.ReturnUrl);
                        return RedirectToAction("Dashboard", "User");
                    }
                    else if (roles.Contains("ADMIN"))
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                }
                else
                {
                    model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl, string role)
        {
            var redirectUrl = Url.Action(
                action: "ExternalLoginCallback",
                controller: "Account",
                values: new { ReturnUrl = returnUrl, role }
            );
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError, string? role)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
                return Content($"<script>alert('Error from external provider: {remoteError}'); window.close();</script>", "text/html");

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return Content($"<script>alert('Error loading external login info.'); window.close();</script>", "text/html");

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                var cuser = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));
                var cuserrole = await _userManager.GetRolesAsync(cuser);
                if (cuserrole.Contains("USER"))
                {
                    return RedirectToAction("Dashboard", "User");
                }
                else if (cuserrole.Contains("EMPLOYEE"))
                {
                    return RedirectToAction("Dashboard", "Employee");
                }
            }

            if (signInResult.IsLockedOut)
                return Content($"<script>alert('Your account is locked out.'); window.close();</script>", "text/html");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? info.Principal.Identity.Name;

            if (email == null)
                return Content($"<script>alert('Email claim not received.'); window.close();</script>", "text/html");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                if (role == "Employee")
                {
                    var serviceTypes = await _serviceTypeGRepository.GetAllAsync();
                    var model = new ExternalRegisterWorkerViewModel
                    {
                        Email = email,
                        UserName = name,
                        Role = "Employee",
                        ServiceType = (List<ServiceType>)serviceTypes
                    };
                    return View("ExternalRegisterWorker", model);
                }
                else
                {
                    user = new ApplicationUser
                    {
                        Email = email,
                        UserName = name
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                        return Content($"<script>alert('Error: {string.Join(", ", createResult.Errors.Select(e => e.Description))}'); window.close();</script>", "text/html");

                    await _userManager.AddLoginAsync(user, info);
                    await _userManager.AddToRoleAsync(user, role.ToUpper());
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Dashboard", "User");
                }
            }
            else
            {
                // user exists but not linked to Google
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Content($"<script>window.opener.location.href = '{returnUrl}'; window.close();</script>", "text/html");
            }
        }

        public async Task<IActionResult> ExternalRegisterWorker()
        {
            var serviceTypes = await _serviceTypeGRepository.GetAllAsync();
            ExternalRegisterWorkerViewModel model = new ExternalRegisterWorkerViewModel
            {
                ServiceType = (List<ServiceType>)serviceTypes
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalRegisterWorker(ExternalRegisterWorkerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Content("<script>alert('External login info not found.'); window.close();</script>", "text/html");
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _userManager.AddLoginAsync(user, info);

            await _userManager.AddToRoleAsync(user, model.Role.ToUpper());
            var serviceman = new ServiceMan
            {
                UserId = user.Id,
                ServiceTypeId = model.ServiceTypeId,
            };
            await _serviceManGRepository.InsertAsync(serviceman);
            await _serviceManGRepository.SaveAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);
            // add into service type
            return RedirectToAction("Dashboard", "Employee");
        }
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("index", "home");
        }
    }
}