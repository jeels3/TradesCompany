using System.Threading.Tasks;
using TradesCompany.Application.Common;
using TradesCompany.Application.DTOs;

namespace TradesCompany.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult> RegisterCustomerAsync(RegisterDto model);
        Task<ServiceResult> RegisterEmployeeAsync(EmployeeRegisterDto model);
        Task<ServiceResult> LoginAsync(LoginDto model);
        Task LogoutAsync();
    }
}
