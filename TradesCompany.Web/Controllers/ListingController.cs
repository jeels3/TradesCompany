using Microsoft.AspNetCore.Mvc;

namespace TradesCompany.Web.Controllers
{
    public class ListingController : Controller
    {
        
        public async Task<IActionResult>  UserListing()
        {
            return View("UserListing");
        }
    }
}
