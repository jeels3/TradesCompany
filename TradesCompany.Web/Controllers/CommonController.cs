using Microsoft.AspNetCore.Mvc;

namespace TradesCompany.Web.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }
    }
}
