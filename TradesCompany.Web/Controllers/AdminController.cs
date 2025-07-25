using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using TradesCompany.Infrastructure.Services;

namespace TradesCompany.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ChartServices _chartServices;

        public AdminController(IUserRepository userRepository, ChartServices chartServices)
        {
            _userRepository = userRepository;
            _chartServices = chartServices;
        }

        public async Task<IActionResult> Dashboard()
        {
            var data = await _chartServices.GetChartData();
            return View(data);
        }

        public async Task<IActionResult> UsersListing()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadUser([FromForm] UserDataTable model)
       {
            var (users, totalRecords) = await _userRepository.GetFilteredUsersAsync(model);

            //// Optional: only send required fields to DataTables
            var results = users.Select(user => new
            {
                user.userId,
                user.UserName,
                user.RoleName,
                user.Email
            });


            return Json(new
            {
                draw = model.Draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = results
            });
        }
    }
}
