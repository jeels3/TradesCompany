using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class BookingByServiceType
    {
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public string? WorkDetails { get; set; }
        public int? bookingId { get; set; }
        public decimal? Price { get; set; }
        public string? img { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
