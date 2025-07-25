using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class ChannelMessage
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        //public int ChannelId { get; set; }
        //public Channel Channel { get; set; }
        public string Message { get; set; }
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public ApplicationUser User { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
