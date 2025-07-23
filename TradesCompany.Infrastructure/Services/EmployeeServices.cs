using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure.Data;

namespace TradesCompany.Infrastructure.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly ApplicationDbContext _context;
        public EmployeeServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<QuotationByServicerMan>> QuotationByServicerMan(string userId)
        {
            return await _context.QuotationByServicerMan
                        .FromSqlInterpolated($"EXEC GetAllQuotationByEmployee {userId}")
                        .ToListAsync();
        }
        public async Task<List<ScheduleServiceByUser>> GettAllScheduleServiceByUser(string userId)
        {
            return await _context.ScheduleServiceByUser.FromSqlInterpolated($"EXEC GetAllScheduleServiceByUser {userId}").ToListAsync();
        }

        public async Task<List<ScheduleServiceByEmployee>> GetAllScheduleServiceByEmployee(string userId)
        {
            return await _context.ScheduleServiceByEmployee.FromSqlInterpolated($"EXEC GetAllScheduleServiceByEmployee {userId}").ToListAsync();
        }

        public async Task<List<string>> GetAllServiceManForSreviceNotification()
        {
            return await _context.ServiceSchedules
                                 .Where(ss => ss.ScheduledAt <= DateTime.Now.AddMinutes(30) && ss.ScheduledAt.AddMinutes(31) >= DateTime.Now && ss.Status != "Completed")
                                 .Select(ss => ss.Id.ToString())
                                 .ToListAsync();
        }

        public async Task<Bill> GetBillByScheduleId(int scheduleId)
        {
            var scheduleservice = await _context.ServiceSchedules.FirstOrDefaultAsync(ss => ss.Id == scheduleId);
            Bill model = new Bill
            {
                Title = scheduleservice.ServiceMan.ServiceTypes.ServiceName,
                serviceCharge = (double)scheduleservice.TotalPrice,
                PlatFormFees = 50,
                Gst = 18,
                TotalPrice = (double)scheduleservice.TotalPrice,
                CreatedAt = DateTime.Now,
            };
            return model;
        }
    } 
}
 