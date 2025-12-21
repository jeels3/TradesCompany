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
        public string QuotationDescription { get; set; }
        public decimal Price { get; set; }
        public Enums.BookingStatus Status { get; set; } = Enums.BookingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
