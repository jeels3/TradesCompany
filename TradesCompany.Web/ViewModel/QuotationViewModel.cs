using System.ComponentModel.DataAnnotations;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.ViewModel
{
    public class QuotationViewModel
    {
        [Required( AllowEmptyStrings = false ,ErrorMessage = "Required")]
        public string Title { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string Description { get; set; } = null!;
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; } = null!;
        public int? ServiceManId { get; set; }
        public ServiceMan? ServiceMan { get; set; } = null!;
        public string? QuotationPdf { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string Status { get; set; } = "Pending"; // ENUM: Pending, Accepted, Rejected
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
