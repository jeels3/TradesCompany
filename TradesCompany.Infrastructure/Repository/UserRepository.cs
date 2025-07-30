using Microsoft.AspNetCore.Identity;
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
using DataTable = TradesCompany.Application.DTOs.UserDataTable;

namespace TradesCompany.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return (Task<IEnumerable<ApplicationUser>>)_userManager.Users;
        }

        //public async Task<List<ApplicationUser>> GetAllCustomerDetailsAsync()
        //{
        //    return await _context.Users.FromSqlRaw("EXEC SelectAllCustomers").ToListAsync();
        //}
        public async Task<List<UsersWithRole>> GetAllUsersAsync()
        {
            //var data = _context.Database.ExecuteSqlAsync($"EXEC GetAllUsersWithRole");
            //var data =  _context.Database.ExecuteSqlAsync($"EXEC GetAllUsersWithRole"); 
            return await _context.UsersWithRole.FromSqlRaw("EXEC GetAllUsersWithRole").ToListAsync();
        }
        public async Task<List<ServiceManByServiceType>> GetAllByServiceTypeServicemenAsync(int ServiceTypeId)
        {
            return await _context.ServiceManByServiceType.FromSqlInterpolated($"EXEC GetAllServicemanByServiceType {ServiceTypeId}").ToListAsync();
        }

        public async Task<(List<UsersWithRole>, int)> GetFilteredUsersAsync(UserDataTable model)
        {
            //var query = _context.UsersWithRole.FromSqlInterpolated($"EXEC GetAllUsersWithRole").AsQueryable();
            var datas = await _context.UsersWithRole.FromSqlInterpolated($"EXEC GetAllUsersWithRole").ToListAsync();
            var query = datas.AsQueryable();
            // Filter
            if (!string.IsNullOrEmpty(model.SearchValue))
            {
                query = query.Where(b =>
                    b.UserName.Contains(model.SearchValue) ||
                    b.RoleName.Contains(model.SearchValue) ||
                    b.Email.Contains(model.SearchValue)
                    );
            }

            int totalRecords = query.Count();

            //if (!string.IsNullOrEmpty(model.SortColumn))
            //{
            //    if (model.SortDirection == "asc")
            //    {
            //        query = query.OrderByDescending(model.SortColumn, true , ?  );
            //    }
            //    else
            //    {
            //        query = query.OrderByDescending(model.SortColumn, false);
            //    }
            //}

            // Paging
            var data = query
                .Skip(model.start)
                .Take(model.length)
                .ToList();

            return (data, totalRecords);
        }

        public async Task<int> GetAllNotificationCount(string userId)
        {
            return _context.Notification.Where(n => n.IsRead == false && n.userId == userId).Count();
        }

        public async Task<List<string>> GetAllUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();
            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Select(c => c.Type).ToList();
        }

        public async Task<Dictionary<string, int>> GetAllDataForAdmin()
        {
            var userCount = await _context.Users.CountAsync();
            var serviceManCount = await _context.ServiceMan.CountAsync();
            var bookingCount = await _context.Bookings.CountAsync();
            var quotationCount = await _context.Quotations.CountAsync();
            var BookingComplete = await _context.ServiceSchedules.CountAsync(n => n.Status == "Completed");
            var TotalRevenue = await _context.ServiceSchedules.Where(ss => ss.Status == "Completed").SumAsync(ss => ss.TotalPrice);
            return new Dictionary<string, int>
            {
                { "UserCount", userCount-serviceManCount },
                { "ServiceManCount", serviceManCount },
                { "BookingCount", bookingCount },
                { "BookingCompleteCount", BookingComplete },
                { "TotalRevenue", (int)TotalRevenue }
            };
        }
    }
}
