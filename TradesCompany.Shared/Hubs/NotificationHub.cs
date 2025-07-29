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
        private readonly IChatRepository _chatRepository;
        private readonly IRepository<IsSeen> _isSeenRepository;
        private readonly INotificationRepository _notificationRepository;
        public NotificationHub(IRepository<Notification> notificationGRepository,
            IRepository<ChannelMessage> channelMessage,
            IChatRepository chatRepository,
            IRepository<IsSeen> isSeenRepository,
            INotificationRepository notificationRepository

            )
        {
            _notificationGRepository = notificationGRepository;
            _channelMessage = channelMessage;
            _chatRepository = chatRepository;
            _isSeenRepository = isSeenRepository;
            _notificationRepository = notificationRepository;
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

        public async Task NotificationCount(string userId)
        {
            var notifications = await _notificationRepository.GetAllNotificationByUserId(userId);
            var unreadCount = notifications.Count(n => !n.IsRead);
            // Send the count to the client
            await Clients.User(userId).SendAsync("ReceiveNotificationCount", unreadCount);
        }
        public async Task ReadNotification(string userId)
        {
            // Get all notification
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

        public async Task ReadMessage(string userId , string ChannelName)
        {
            var messages = await _chatRepository.GetChatMessageByChannelName(ChannelName);

            foreach (var message in messages)
            {
                if (message.SenderId != userId)
                {
                    // check if the message is already seen
                    var isSeendata = await _isSeenRepository.GetAllAsync();
                    if(isSeendata.Any(i => i.ReceiverId == userId && i.ChannelMessageId == message.Id))
                    {
                        continue; // Skip if already seen
                    }
                    else
                    {
                        IsSeen isSeen = new IsSeen
                        {
                            Seen = true,
                            SeenDate = DateTime.Now,
                            ReceiverId = userId,
                            ChannelMessageId = message.Id,
                        };
                    await _isSeenRepository.InsertAsync(isSeen);
                    }
                }
            }
            await _isSeenRepository.SaveAsync();
        }
        public async Task SendMessage(string message , string SenderId , string ChannelName)
        {
            var channelId = await _chatRepository.GetChannelIdByChannelName(ChannelName);
            if (channelId == 0)
            {
                throw new Exception("Channel does not exist.");
            }

            ChannelMessage model = new ChannelMessage
            {
                ChannelName = ChannelName,
                Message = message,
                SenderId = SenderId,
                ChannelId = channelId,
            };

            await _channelMessage.InsertAsync(model);
            await _channelMessage.SaveAsync();
            await Clients.Group(ChannelName)
                .SendAsync("RecieveMessage", message , SenderId);
        }

        public async Task ChatNotificationCount(string userId)
        {
            var notifications = await _isSeenRepository.GetAllAsync();
            var userChannels = await _chatRepository.GetAllUserListing(userId);
            var unreadCount = notifications.Count(n => n.ReceiverId == userId && !n.Seen);
            await Clients.User(userId).SendAsync("ReceiveChatNotificationCount", unreadCount);
        }


        // Automatic disconnect when user change page or view the client side
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"User disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
