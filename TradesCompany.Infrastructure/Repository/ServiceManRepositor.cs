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
    public class ServiceManRepositor : IServiceManRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceManRepositor(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceMan> GetServiceManByUserId(string userId)
        {
            return await _context.ServiceMan.Where(s => s.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
