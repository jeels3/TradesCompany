using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class ServiceManByServiceType
    {
        public string? ServiceName { get; set; }
        public string? userId { get; set; }
        public string? UserName { get; set; }
        public string? Email {  get; set; }
        public double? ratings { get; set; }
        public string? Feedback { get; set; }
    }
}
