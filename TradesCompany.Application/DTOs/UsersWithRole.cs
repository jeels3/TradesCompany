using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.DTOs
{
    public class UsersWithRole
    {
        public string userId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string roleId { get; set; }
        public string RoleName { get; set; }
    }
}
