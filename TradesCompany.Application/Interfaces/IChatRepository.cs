using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.Interfaces
{
    public interface IChatRepository
    {
        Task<List<ApplicationUser>> GetAllUserListing(string userId);
        Task<bool> CheckChennelNameIsExists(string ChannelName);
        Task<List<ChannelMessage>> GetChatMessageByChannelName (string ChannelName);
    }
}
