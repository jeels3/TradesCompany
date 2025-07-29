using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
            return 1;
        }

        public async Task<List<Channel>> GetGroupByUserId(string userId)
        {
            return await _context.Channel
                .Where(c => c.ChannelUsers.Any(cu => cu.UserId == userId) && c.ChannelName.StartsWith("Group"))
                .ToListAsync();
        }
    }
}
