using Microsoft.AspNetCore.Mvc;
using TradesCompany.Application.Interfaces;
namespace TradesCompany.Web.ViewComponents
{
    public class UserByServiceTypeViewComponent : ViewComponent
    {
      private readonly IUserRepository _userRepository;
        public UserByServiceTypeViewComponent(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int serviceTypeId)
        {
            var servicemen = await _userRepository.GetAllByServiceTypeServicemenAsync(serviceTypeId);
            return View(servicemen);
        }


    }
}