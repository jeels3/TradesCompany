using Microsoft.AspNetCore.Mvc;
using TradesCompany.Application.Interfaces;

namespace TradesCompany.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
    }
}
