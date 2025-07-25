using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using DataTable = TradesCompany.Application.DTOs.UserDataTable;

namespace TradesCompany.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly IUserRepository _userRepository;
        public CommonController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> ShowNotificationCount()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var count = await _userRepository.GetAllNotificationCount(userId);
                return Json(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Authorize(Policy = "Permission.BookListing")]
        //public async Task<IActionResult> GetServiceMenListingByServiceType([FromForm] DataTable model)
        //{
        //    // Use your repository to get data
        //    var (Employee, totalRecords) = await _userRepository.GetFilteredEmployee(model);

        //    var data = Employee.Select(e => new
        //    {
        //        UserName = e.UserName,
        //        Email = e.Email,
        //    });

        //    return Json(new
        //    {
        //        draw = model.Draw,
        //        recordsTotal = totalRecords,
        //        recordsFiltered = totalRecords,
        //        data = data
        //    });
        //}
    }
}
