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
        public decimal serviceCharge { get; set; }
        public decimal Gst {  get; set; }
        public decimal PlatFormFees { get; set; }
        public decimal TotalPrice { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int serviceScheduleId { get; set; }
        public ServiceSchedule? serviceSchedule { get; set; }
    }
}
