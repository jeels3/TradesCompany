using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradesCompany.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        [Authorize(Policy = "BookingServicePolicy")]
        public IActionResult Bookings()
        {
            return View();
        }
    }
}
