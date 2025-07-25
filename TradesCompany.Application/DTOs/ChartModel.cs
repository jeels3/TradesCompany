using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class ChartModel
    {
        public List<DataPoint> Data { get; set; }

        public class DataPoint
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }
    }
}
