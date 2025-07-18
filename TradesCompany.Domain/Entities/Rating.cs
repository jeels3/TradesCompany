using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int ServiceScheduleId { get; set; }
        public ServiceSchedule ServiceSchedule { get; set; } = null!;

        public decimal Stars { get; set; }
        public string Feedback { get; set; } = null!;

    }
}
