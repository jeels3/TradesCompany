using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TradesCompany.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; } = null!;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public string WorkDetails { get; set; } = null!;
        public string? QuotationPdf { get; set; } // optional you can add after service completed
        public string? imagepath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; } = "Pending"; // ENUM: Pending, Accepted, Rejected, Completed
        public ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
        public ICollection<ServiceSchedule>? ServiceSchedules { get; set; } = new List<ServiceSchedule>();
    }
}
