using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public ICollection<Address> Addresses { get; set; }  = new List<Address>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ServiceMan? ServiceMan { get; set; }
    }
}
