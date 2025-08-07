using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotificationByUserId(string userId);
    }
}
