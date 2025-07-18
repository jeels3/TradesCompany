using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class ServiceSchedule
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        public int? ServiceManId { get; set; }
        public ServiceMan ServiceMan { get; set; } = null!;
        public int? QuotationId { get; set; }
        public Quotation Quotation { get; set; } = null!;

        public DateTime ScheduledAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Scheduled"; // ENUM: Scheduled, Completed, Cancelled
        public int? BillId { get; set; }

        public Bill? Bill { get; set; }
        public Rating? Rating { get; set; }
    }
}
