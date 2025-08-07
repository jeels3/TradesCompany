using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class IsSeen
    {
        public int Id { get; set; }
        public int ChannelMessageId { get; set; }
        public ChannelMessage ChannelMessage { get; set; }
        public bool Seen { get; set; } = false;
        public DateTime? SeenDate { get; set; } = null;
        public string ReceiverId { get; set; }
    }
}
