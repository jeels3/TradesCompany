using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.Services
{
    public interface INotificationService
    {
        Task SendNotificationOfNewBooking(int ServiceTypeId , string NotificationType , string Message);
        Task sendhelo();
        Task SendNotificationOfNewQuotation(string userId  , string NotificationType , string Message);
        Task SendNotificationOfScheduleService(string userId , string NotificationType , string Message);
        Task SendNotificationOfScheduleServiceToEmployee(string userId, string NotificationType, string Message);
    }
}

