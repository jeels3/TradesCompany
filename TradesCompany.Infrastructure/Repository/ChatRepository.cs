using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure.Data;
using TradesCompany.Infrastructure.Services;

namespace TradesCompany.Infrastructure.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ConnectionService _connectionService;

        public ChatRepository(ApplicationDbContext context, ConnectionService connectionService)
        {
            _context = context;
            _connectionService = connectionService;
        }

        public async Task<List<ApplicationUser>> GetAllUserListing(string userId)
        {
            return _context.Users.Where(u => u.Id != userId).ToList();
        }

        public async Task<bool> CheckChennelNameIsExists(string ChannelName)
        {
            var channelname = await _context.Channel.Where(c => c.ChannelName == ChannelName).ToListAsync();
            if (channelname.Count != 0)
            {
                return true;
            }
            return false;
        }
        public async Task<List<ChannelMessage>> GetChatMessageByChannelName(string ChannelName)
        {
            return await _context.ChannelMessage
                .Where(ch => ch.ChannelName == ChannelName)
                .Include(cm => cm.User)
                .Include(cm => cm.IsSeen)
                .OrderBy(ch => ch.CreateAt) // Order by CreateAt in descending order
                .ToListAsync();
        }
        public async Task<int> GetChannelIdByChannelName(string ChannelName)
        {
            var channel = await _context.Channel.FirstOrDefaultAsync(c => c.ChannelName == ChannelName);
            if (channel != null)
            {
                return channel.Id;
            }
            return 0; // or throw an exception if preferred
        }

        public async Task<bool> CheckUserInChannel(int channelId, string userId)
        {
            return await _context.ChannelUser.AnyAsync(cu => cu.ChannelId == channelId && cu.UserId == userId);
        }

        public async Task<int> GetAllUnreadMessageByUserId(string userId)
        {
            var unreadCount = _context.ChannelMessage
                            .Where(cm => cm.SenderId != userId) // Not sent by the user
                            .Where(cm => cm.Channel.ChannelUsers.Any(cu => cu.UserId == userId)) // User is part of the channel
                            .Where(cm => !cm.IsSeen.Any(s => s.ReceiverId == userId)) // No seen record = unseen
                            .Count();

            return unreadCount;
        }

        public async Task<int> GetAllUnreadMessageByChannelId(int channelId, string userId)
        {
            return _context.ChannelMessage
                            .Where(cm => cm.ChannelId == channelId)
                            .Where(cm => cm.SenderId != userId)
                            .Where(cm => !cm.IsSeen.Any(s => s.ReceiverId == userId)) // No IsSeen row for this user = not seen
                            .Count();
        }

        public async Task<UnreadMessageInfoDto> GetAllUnreadMessageByChannelIdWithSenderDetails(int channelId, string userId)
        {
            // Step 1: Get the other participant (sender)
            var senderId = await _context.ChannelUser
                .Where(cu => cu.ChannelId == channelId && cu.UserId != userId)
                .Select(cu => cu.UserId)
                .FirstOrDefaultAsync();

            if (senderId == null)
                return null; // No other user found in this channel

            // Step 2: Get count of unread messages from that sender to this user
            var unreadCount = await _context.ChannelMessage
                .Where(cm => cm.ChannelId == channelId)
                .Where(cm => cm.SenderId == senderId)
                .Where(cm => !cm.IsSeen.Any(s => s.ReceiverId == userId))
                .CountAsync();

            // Step 3: Always return senderId and count (even if 0)
            return new UnreadMessageInfoDto
            {
                SenderId = senderId,
                UnreadCount = unreadCount
            };
        }

        public async Task<List<Channel>> GetGroupByUserId(string userId)
        {
            return await _context.Channel
                .Where(c => c.ChannelUsers.Any(cu => cu.UserId == userId) && c.ChannelName.StartsWith("Group"))
                .ToListAsync();
        }

        public async Task<List<string>> GetUserByChannelId(int channelId)
        {
            return await _context.ChannelUser
                .Where(cu => cu.ChannelId == channelId)
                .Select(cu => cu.UserId)
                .ToListAsync();
        }

        public async Task<List<UserAndGroupListingWithCount>> GetAllChannelsByUserId(string userId)
        {
            return null;
        }

        public async Task<List<UserAndGroupListingWithCount>> GetUserAndGroupListingWithCount(string userId)
        {
            var userChannels = await _context.ChannelUser
                .Where(cu => cu.UserId == userId)
                .Select(cu => cu.Channel)
                .ToListAsync();
            var result = new List<UserAndGroupListingWithCount>();
            foreach (var channel in userChannels)
            {
                var unreadCount = await GetAllUnreadMessageByChannelId(channel.Id, userId);
                var channelName = await _context.Channel.Where(c => c.Id == channel.Id).FirstOrDefaultAsync();
                var unreadcountforpersonal = await GetAllUnreadMessageByChannelIdWithSenderDetails(channel.Id, userId);

                if (channelName.ChannelName.Contains("Group"))
                {
                    GroupDTO group = new GroupDTO
                    {
                        ChannelName = channelName.ChannelName,
                        uReadCount = unreadCount
                    };
                    result.Add(new UserAndGroupListingWithCount
                    {
                        Groups = new List<GroupDTO> { group }
                    });
                }
                else
                {
                    UserDto userDto = new UserDto
                    {
                        UserId = unreadcountforpersonal.SenderId,
                        UserName = await _context.Users.Where(u => u.Id == unreadcountforpersonal.SenderId).Select(u => u.UserName).FirstOrDefaultAsync(),
                        uReadCount = unreadcountforpersonal.UnreadCount
                    };
                    result.Add(new UserAndGroupListingWithCount
                    {
                        Users = new List<UserDto> { userDto }
                    });
                }
            }

            if(result.Count == 0)
            {
                // Add All Users Listing Here 
                var users = await _context.Users.Where(u => u.Id != userId).ToListAsync();
                foreach(var user in users)
                {
                    UserDto userDto = new UserDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        uReadCount =  0
                    };
                    result.Add(new UserAndGroupListingWithCount
                    {
                        Users = new List<UserDto> { userDto }
                    });
                }
            }
            return result;
        }
    }
}
//---->  Get Unread Count By ReceiverId and SenderId
 