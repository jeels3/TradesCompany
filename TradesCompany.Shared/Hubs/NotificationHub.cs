using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Shared.Hubs
{
    public  class NotificationHub : Hub
    {
        private readonly IRepository<Notification> _notificationGRepository;
        private readonly IRepository<ChannelMessage> _channelMessage;
        public NotificationHub(IRepository<Notification> notificationGRepository, IRepository<ChannelMessage> channelMessage)
        {
            _notificationGRepository = notificationGRepository;
            _channelMessage = channelMessage;
        }
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"Connection {Context.ConnectionId} joined group {groupName}");
        }
        public async Task LeaveGroup(string groupName)  
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task ReadNotification(string userId)
        {
            var notifications = await _notificationGRepository.GetAllAsync();
            foreach (var notification in notifications)
            {
                if(notification.userId == userId)
                {
                    notification.IsRead = true;
                }
            }
            await _notificationGRepository.SaveAsync();
        }

        public async Task SendMessage(string message , string SenderId , string ChannelName)
        {
            Console.WriteLine(message);
            ChannelMessage model = new ChannelMessage
            {
                ChannelName = ChannelName,
                Message = message,
                SenderId = SenderId,
            };
            await _channelMessage.InsertAsync(model);
            await _channelMessage.SaveAsync();
            await Clients.Group(ChannelName)
                .SendAsync("RecieveMessage", message , SenderId);
        }


        // Automatic disconnect when user change page or view the client side
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"User disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
