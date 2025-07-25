using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class ChannelMessage
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public Channel Channel { get; set; }
        public string Message { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsRead { get; set; }
    }
}
