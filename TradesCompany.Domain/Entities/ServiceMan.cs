using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TradesCompany.Domain.Entities
{
    public class ServiceMan
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceTypes { get; set; }
        public ICollection<Quotation>? Quotations { get; set; } = new List<Quotation>();
        public ICollection<ServiceSchedule>? ServiceSchedules { get; set; } = new List<ServiceSchedule>();
    }
}
