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
        public NotificationHub(IRepository<Notification> notificationGRepository)
        {
            _notificationGRepository = notificationGRepository;
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

        // Automatic disconnect when user change page or view the client side
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"User disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
