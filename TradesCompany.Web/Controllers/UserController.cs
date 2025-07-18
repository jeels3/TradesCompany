using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<ServiceType> _serviceGRepository;

        public UserController(IRepository<ServiceType> serviceGReposuitory)
        {
            _serviceGRepository = serviceGReposuitory;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var servicesData = await _serviceGRepository.GetAllAsync();
                return View(servicesData);
            }
            catch(Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }
        }
    }
}
