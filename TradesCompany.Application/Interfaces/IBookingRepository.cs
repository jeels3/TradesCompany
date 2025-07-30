using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<BookingByServiceType>> GetAllBookingAsync(int ServiceTypeId);
        Task<List<BookingByServiceType>> GetAllNewBookingAsync();
        Task <BookingByServiceType> GetBookingByAsync(int id);
        Task<List<Booking>> GetAllBookingByUserId(string userId);
    }
}
