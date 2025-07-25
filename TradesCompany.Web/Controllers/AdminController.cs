using ClosedXML.Excel;
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
        private readonly ExcelService excelService;

        public AdminController(IUserRepository userRepository, ChartServices chartServices, ExcelService excelService)
        {
            _userRepository = userRepository;
            _chartServices = chartServices;
            this.excelService = excelService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var data = await _chartServices.GetChartData();
            return View(data);
        }

        //public async Task<IActionResult> DownloadExcel([FromForm] ChartModel model)
        //{
        //    Console.WriteLine(model);
        //    excelService.GenerateExcelFile(model);
        //    return Ok();
        //}

        [HttpPost]
        public IActionResult DownloadExcel([FromForm] ChartModel model)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Data");

                // Add headers
                var rowcnt = 1;
                foreach (var row in model.Data)
                {
                    worksheet.Cell(rowcnt, 1).Value = row.Label;
                    worksheet.Cell(rowcnt, 2).Value = row.Value;
                    rowcnt++;
                }
                worksheet.Columns().AdjustToContents();
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0; // Reset stream position for reading

                // 4. Return the file as a FileContentResult
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MyData.xlsx");
            }
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
