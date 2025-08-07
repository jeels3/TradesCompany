using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ImgLink { get; set; } = string.Empty;
        public ICollection<ServiceMan> ServiceMen { get; set; } = new List<ServiceMan>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
