using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.Services
{
    public interface IEmployeeServices
    {
        Task<List<QuotationByServicerMan>> QuotationByServicerMan(string userId);
        Task<List<ScheduleServiceByUser>> GettAllScheduleServiceByUser(string userId);
    }
}
