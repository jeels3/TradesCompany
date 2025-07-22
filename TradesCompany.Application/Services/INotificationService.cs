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
    }
}
