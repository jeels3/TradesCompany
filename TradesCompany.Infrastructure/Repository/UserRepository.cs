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
using DataTable = TradesCompany.Application.DTOs.DataTable;

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
            return await _context.UsersWithRole.FromSqlRaw("EXEC GetAllUsersWithRole").ToListAsync();
        }

        public async Task<List<ServiceManByServiceType>> GetAllByServiceTypeServicemenAsync(int ServiceTypeId)
        {
            return await _context.ServiceManByServiceType.FromSqlInterpolated($"EXEC GetAllServicemanByServiceType {ServiceTypeId}").ToListAsync();
        }

        //public async Task<(List<ApplicationUser>, int)> GetFilteredEmployee(DataTable model)
        //{
           
        //}
    }
}
