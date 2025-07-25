using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class ChannelUser
    {
        public string ChannelName { get; set; }
        public Channel Channel { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

