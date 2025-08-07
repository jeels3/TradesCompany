using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class Channel
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public string CreatorId { get; set; }
        public ApplicationUser User { get; set; }
        public List<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();
    }
}
