using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure.Data;

namespace TradesCompany.Infrastructure.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public ScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Schedule>> GetAllScheduleSlotByServiceManId(string userId)
        {
            var serviceman = await _context.ServiceMan.Where(sm => sm.UserId == userId).FirstOrDefaultAsync();
            return await _context.Schedule.Where(s => s.ServiceManId == serviceman.Id).ToListAsync();
        }
    }
}
