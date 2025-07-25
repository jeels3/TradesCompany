using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure.Data;

namespace TradesCompany.Infrastructure.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetAllUserListing(string userId)
        {
            return _context.Users.Where(u => u.Id != userId).ToList();
        }

        public async Task<bool> CheckChennelNameIsExists(string ChannelName)
        {
            var channelname = await _context.Channel.Where(c => c.ChannelName == ChannelName).ToListAsync();
            if(channelname.Count != 0)
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
                .OrderBy(ch => ch.CreateAt)
                .ToListAsync();
        }
    }
}
