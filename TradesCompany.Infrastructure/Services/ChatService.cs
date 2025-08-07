
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Shared.Hubs;

namespace TradesCompany.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IRepository<ChannelMessage> _channelMessage;

        public ChatService(IHubContext<NotificationHub> hubContext, IRepository<ChannelMessage> channelMessage)
        {
            _hubContext = hubContext;
            _channelMessage = channelMessage;
        }

        public async Task SendMessage(string message, string ChannelName, string userId)
        {
            ChannelMessage model = new ChannelMessage
            {
                ChannelName = ChannelName,
                Message = message,
                SenderId = userId,
            };
            await _channelMessage.InsertAsync(model);
            await _channelMessage.SaveAsync();
            await _hubContext.Clients.Group(ChannelName)
                .SendAsync("RecieveMessage", message);
        }

    }
}
