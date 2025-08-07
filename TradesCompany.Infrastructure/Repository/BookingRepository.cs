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
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<BookingByServiceType>> GetAllBookingAsync(int ServiceTypeId)
        {
            return await _context.BookingByServiceType
                .FromSqlInterpolated($"EXEC GetAllBookingByServiceType {ServiceTypeId}")
                .ToListAsync();
        }
        public async Task<List<BookingByServiceType>> GetAllNewBookingAsync()
        {
            return await _context.BookingByServiceType
                .ToListAsync();
        }
        public async Task<BookingByServiceType> GetBookingByAsync(int id)
        {
            return await _context.BookingByServiceType
                .FirstOrDefaultAsync();
        }

        public async Task<List<Booking>> GetAllBookingByUserId(string userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.ServiceType)
                .ToListAsync();
        }
    }
}
