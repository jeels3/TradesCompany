using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class UnreadMessageInfoDto
    {
        public string? SenderId { get; set; }
        public int? UnreadCount { get; set; }
    }
}
