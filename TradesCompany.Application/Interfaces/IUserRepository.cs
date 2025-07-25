  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Domain.Entities;
using DataTable = TradesCompany.Application.DTOs.UserDataTable;

namespace TradesCompany.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        //Task <List<ApplicationUser>> GetAllCustomerDetailsAsync();
        Task <List<UsersWithRole>> GetAllUsersAsync();
        Task<List<ServiceManByServiceType>> GetAllByServiceTypeServicemenAsync(int ServiceTypeId);
        Task<(List<UsersWithRole>, int)> GetFilteredUsersAsync(UserDataTable model);
    }
}
