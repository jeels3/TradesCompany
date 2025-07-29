using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.ViewModel
{
    public class UserAndGroupListing
    {
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<Channel> channels { get; set; } = new List<Channel>();
    }
}
