using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class QuotationByUser
    {
        public int quotationId {  get; set; }
        public string? userId {  get; set; }
        public string? UserName { get; set; }
        public decimal? Price { get; set; }
        public string? ServicemanName { get; set; }
        public string? ServiceManEmail { get; set; }
        public string? ServiceName { get; set; }
        public string? WorkDetails {  get; set; }
        public string? status { get; set; }
        public string? Description { get; set; }
    }
}
