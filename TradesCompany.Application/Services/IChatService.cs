using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;

namespace TradesCompany.Application.Services
{
    public interface IChatService
    {
        Task SendMessage(string message , string ChannelName , string userId);

    }
}
