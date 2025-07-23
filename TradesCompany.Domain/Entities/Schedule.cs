using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly Time { get; set; }
        public int ServiceManId { get; set; }
        public ServiceMan ServiceMan { get; set; }
    }
}
