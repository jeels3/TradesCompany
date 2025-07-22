using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class Quotation
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; } = null!;
        public int? ServiceManId { get; set; }
        public ServiceMan? ServiceMan { get; set; } = null!;
        public string QuotationPdf { get; set; } = null!;
        public decimal Price { get; set; }
        public string Status { get; set; } = "Pending"; // ENUM: Pending, Accepted, Rejected
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
