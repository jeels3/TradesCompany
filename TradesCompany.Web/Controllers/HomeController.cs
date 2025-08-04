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
using TradesCompany.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TradesCompany.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       private readonly IRepository<ServiceMan> repository;
        private readonly IEmployeeServices employeeServices;
        private readonly EmailService emailService;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger  , IRepository<ServiceMan> repository,IEmployeeServices employeeServices,EmailService emailService
                              , ApplicationDbContext context
                               )
        {
            _logger = logger;
            this.repository = repository;
            this.employeeServices = employeeServices;
            this.emailService = emailService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
            var data = await _context.Database.ExecuteSqlAsync($"EXEC GetAllUsersWithRole");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
