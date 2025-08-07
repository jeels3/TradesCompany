using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class ScheduleDtos
    {
        public int quotationId { get; set; }
        public string? date { get; set; }
        public string? time { get; set; }
    }
}
