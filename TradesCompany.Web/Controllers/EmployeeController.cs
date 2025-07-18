using Microsoft.AspNetCore.Mvc;

namespace TradesCompany.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
