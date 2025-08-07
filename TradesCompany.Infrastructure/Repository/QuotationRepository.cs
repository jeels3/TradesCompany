using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure.Data;

namespace TradesCompany.Infrastructure.Repository
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly ApplicationDbContext _context;

        public QuotationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<QuotationByUser>> GetQuotationForUser(string userId)
        {
            return await _context.QuotationByUser
                .FromSqlInterpolated($"EXEC GetAllQuotationForUser {userId}")
                .ToListAsync();
        }

        public async Task<List<Quotation>> GetQuotationByBookingId(int bookingId)
        {
            return await _context.Quotations
                .Where(q => q.BookingId == bookingId)
                .ToListAsync();
        }
    }
}
 
