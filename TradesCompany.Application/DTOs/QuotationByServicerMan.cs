using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class QuotationByServicerMan
    {
        public int quotationId { get; set; }
        public string customerId { get; set; }
        public string? userId { get; set; }
        public string? CustomerName { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }
        public string? ServiceName { get; set; }
        public string? WorkDetails { get; set; }
    }
}
