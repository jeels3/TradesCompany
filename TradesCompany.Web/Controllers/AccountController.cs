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
       private readonly  UserManager<ApplicationUser> _userManager;
       private readonly SignInManager<ApplicationUser> _signInManager;
       private readonly RoleManager<ApplicationRole> _roleManager;
       private readonly IRepository<ServiceType> _serviceTypeGRepository;
        private readonly IRepository<ServiceMan> _serviceManGRepository;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IRepository<ServiceType> serviceTypeGRepository,
            IRepository<ServiceMan> serviceManGRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _serviceTypeGRepository = serviceTypeGRepository;
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
            EmployeeRegisterViewModel model = new EmployeeRegisterViewModel
            {
                ServiceTypes = (List<ServiceType>)serviceTypes,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CustomerRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
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
                    return RedirectToAction("Dashboard","User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegistrationError", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EmployeeRegister(EmployeeRegisterViewModel model)
        {
            if (ModelState.IsValid)
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
                    await _serviceManGRepository.InsertAsync(serviceman);
                    await _serviceManGRepository.SaveAsync();
                    return RedirectToAction("Dashboard", "Employee");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegistrationError", error.Description);
                }
            }
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
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("EMPLOYEE "))
                    { 
                        if(model.ReturnUrl != null) return Redirect(model.ReturnUrl);
                        return RedirectToAction("Dashboard", "Employee");
                    }
                    else if (roles.Contains("USER"))
                    {
                        if (model.ReturnUrl != null) return Redirect(model.ReturnUrl);
                        return RedirectToAction("Dashboard", "User");
                    }
                    //return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                if (result.RequiresTwoFactor)
                {
                    // Handle two-factor authentication case
                }
                if (result.IsLockedOut)
                {
                    // Handle lockout scenario
                }
                else
                {
                    // Handle failure
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> IsEmailAvailable(string Email)
        {
            //Check If the Email Id is Already in the Database
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use.");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl , string role)
        {
            var redirectUrl = Url.Action(
                action: "ExternalLoginCallback",
                controller: "Account",           
                values: new { ReturnUrl = returnUrl , role} 
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
                return RedirectToAction("Dashboard", "User");
                return Content($"<script>window.opener.location.href = '{returnUrl}'; window.close();</script>", "text/html");
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
                    var model = new ExternalRegisterWorkerViewModel
                    {
                        Email = email,
                        UserName = name,
                        Role = "Employee"
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
                    await _userManager.AddToRoleAsync(user,role);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Dashboard", "User");
                    return Content($"<script>window.opener.location.href = '{returnUrl}'; window.close();</script>", "text/html");
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

            await _userManager.AddToRoleAsync(user, model.Role);
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
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}
