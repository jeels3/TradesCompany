using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public double serviceCharge { get; set; }
        public double Gst {  get; set; }
        public double PlatFormFees { get; set; }
        public double TotalPrice { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
