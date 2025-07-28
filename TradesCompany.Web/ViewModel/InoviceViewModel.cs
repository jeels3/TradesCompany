using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.ViewModel
{
    public class InoviceViewModel
    {
        public int BillId { get; set; }
        public Bill? Bill { get; set; }
        public string CustomerName { get; set; }
        public string ServiceManName { get; set; }
    }
}
