using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.DTOs
{
    public class UserAndGroupListingWithCount
    {
       public List<GroupDTO>? Groups { get; set; } = new List<GroupDTO>();
        public List<UserDto>? Users { get; set; } = new List<UserDto>();
    }
    public class GroupDTO
    {
        public string? ChannelName { get; set; }
        public int? uReadCount { get; set; } = 0;
    }
    public class UserDto
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public int? uReadCount { get; set; } = 0;
    }
}
