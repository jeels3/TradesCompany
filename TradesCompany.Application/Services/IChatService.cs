using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Application.Services
{
    public interface IChatService
    {
        Task SendMessage(string message , string ChannelName , string userId);
    }
}
