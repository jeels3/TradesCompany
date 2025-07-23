using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Shared.Hubs;

namespace TradesCompany.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Notification> _notificationGRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(IUserRepository userRepository,
                                    IRepository<Notification> notificationGRepository,
                                    IHubContext<NotificationHub> hubContext)
        {
            _userRepository = userRepository;
            _notificationGRepository = notificationGRepository;
            _hubContext = hubContext;
        }

        public async Task sendhelo()
        {
            await _hubContext.Clients.All.SendAsync("Hello", "Hello");
        }
        public async Task SendNotificationOfNewBooking(int ServiceTypeId, string NotificationType, string Message)
        {
            
            // Get All User Which Is this ServiceTypeId
            var users =await _userRepository.GetAllByServiceTypeServicemenAsync(ServiceTypeId);
            foreach (var user in users)
            {
                Notification model = new Notification
                {
                    userId = user.userId,
                    NotificationType = NotificationType,
                    Message = Message
                };
                await _notificationGRepository.InsertAsync(model);
                await _notificationGRepository.SaveAsync();
                await _hubContext.Clients.Group($"UserGroup_{user.userId}")
                                        .SendAsync("ReceiveBookingNotification", NotificationType, Message);
            }
        }

        public async Task SendNotificationOfNewQuotation(string userId , string NotificationType, string Message)
        {
            Notification model = new Notification
            {
                userId = userId,
                NotificationType = NotificationType,
                Message = Message
            };
            await _notificationGRepository.InsertAsync(model);
            await _notificationGRepository.SaveAsync();
            await _hubContext.Clients.Group($"UserGroup_{userId}")
                                        .SendAsync("ReceiveNewQuotation", NotificationType, Message);

        }
        public async Task SendNotificationOfScheduleService(string userId, string NotificationType, string Message)
        {
            Notification model = new Notification
            {
                userId = userId,
                NotificationType = NotificationType,
                Message = Message
            };
            await _notificationGRepository.InsertAsync(model);
            await _notificationGRepository.SaveAsync();
            await _hubContext.Clients.Group($"UserGroup_{userId}")
                                        .SendAsync("ReceiveNewSchedule", NotificationType, Message);
        }
        
        public async Task SendNotificationOfScheduleServiceToEmployee(string userId, string NotificationType, string Message)
        {
            Notification model = new Notification
            {
                userId = userId,
                NotificationType = NotificationType,
                Message = Message
            };
            await _notificationGRepository.InsertAsync(model);
            await _notificationGRepository.SaveAsync();
            await _hubContext.Clients.Group($"UserGroup_{userId}")
                                        .SendAsync("RecieveScheduleSeviceReminder", NotificationType, Message);
        }
    }
}
