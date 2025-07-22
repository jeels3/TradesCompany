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
    }
}
 