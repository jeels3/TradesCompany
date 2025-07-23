using TradesCompany.Application.DTOs;

namespace TradesCompany.Web.ViewModel
{
    public class AllQuotationEmployeeViewModel
    {
        //public List<QuotationByServicerMan> QuotationByServicerMan { get; set; } = new List<QuotationByServicerMan>();
        public DateOnly? date {  get; set; }
        public TimeOnly? time { get; set; }
        public int? quotationId { get; set; }
    }
}
