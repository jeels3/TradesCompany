using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class ScheduleServiceByUser
    {
        public int ScheduleServiceId { get; set; }
        public string? WorkDetails { get; set; }
        public string? ServiceMan {  get; set; }
        public string? ServiceManUserId { get; set; }
        public string? ServiceName { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
