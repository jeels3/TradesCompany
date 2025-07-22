using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.ViewModel
{
    public class BookingViewModel
    {
        public int ServiceTypeId { get; set; }
        public string WorkDetails { get; set; } = null!;
        public string? imagepath { get; set; }
        public int price { get; set; }
        public List<ServiceType>? ServiceTypes { get; set; } = new List<ServiceType>();
    }
}
