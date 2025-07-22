using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Shared.Hubs;
using TradesCompany.Web.Models;

namespace TradesCompany.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<ServiceMan> repository;
        public HomeController(ILogger<HomeController> logger  , IRepository<ServiceMan> repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var serviceman = repository.GetAllAsync();
            Console.WriteLine(serviceman.Id);
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
