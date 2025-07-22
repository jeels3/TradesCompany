using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public bool IsRead { get; set; }  = false;
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
