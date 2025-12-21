using System.ComponentModel.DataAnnotations;

namespace TradesCompany.Application.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class EmployeeRegisterDto : RegisterDto
    {
        public int ServiceTypeId { get; set; }
    }
}
