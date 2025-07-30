using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.Interfaces
{
    public interface IChatRepository
    {
        Task<List<ApplicationUser>> GetAllUserListing(string userId);
        Task<bool> CheckChennelNameIsExists(string ChannelName);
        Task<List<ChannelMessage>> GetChatMessageByChannelName(string ChannelName);
        Task<int> GetChannelIdByChannelName(string ChannelName);
        Task<bool> CheckUserInChannel(int channelId, string userId);
        Task<List<Channel>> GetGroupByUserId(string userId);
        Task<int> GetAllUnreadMessageByUserId(string userId);
        Task <int> GetAllUnreadMessageByChannelId(int channelId, string userId);
        Task<List<string>> GetUserByChannelId(int channelId);
        Task<List<UserAndGroupListingWithCount>> GetAllChannelsByUserId(string userId);
    }
}
