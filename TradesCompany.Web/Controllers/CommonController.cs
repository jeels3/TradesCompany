using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using DataTable = TradesCompany.Application.DTOs.DataTable;

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
