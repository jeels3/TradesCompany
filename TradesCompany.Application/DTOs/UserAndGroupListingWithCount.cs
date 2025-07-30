using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.DTOs
{
    public class UserAndGroupListingWithCount
    {
        public List<UserAndGroupListingViewModel> userAndGroupListingViewModels { get; set; } = new List<UserAndGroupListingViewModel>();
    }
    public class UserAndGroupListingViewModel
    {
        public ApplicationUser User { get; set; }
        public Channel Channels { get; set; }
        public int uReadCount { get; set; }
    }
    
}
