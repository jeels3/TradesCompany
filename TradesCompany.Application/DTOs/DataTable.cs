using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class DataTable
    {
        public int Draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string SearchValue { get; set; }
        public  int ServiceTypeId { get; set; }

        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}
