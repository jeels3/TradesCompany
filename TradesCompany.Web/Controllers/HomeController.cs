using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Shared.Hubs;
using TradesCompany.Web.Models;
using Microsoft.AspNetCore.SignalR;
using TradesCompany.Application.Services;
using TradesCompany.Infrastructure.Services;

namespace TradesCompany.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       private readonly IRepository<ServiceMan> repository;
        private readonly IEmployeeServices employeeServices;
        private readonly EmailService emailService;
        public HomeController(ILogger<HomeController> logger  , IRepository<ServiceMan> repository,IEmployeeServices employeeServices,EmailService emailService
                              
                               )
        {
            _logger = logger;
            this.repository = repository;
            this.employeeServices = employeeServices;
            this.emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            //var data = await employeeServices.GetAllServiceManForSreviceNotification();
            //await emailService.SendEmailAsync("jeell372004@gmail.com", "check", "<h1>hello</h1>", true);
            return View();
        }

        public IActionResult Privacy()
        {
            throw new Exception ("hello ji");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
