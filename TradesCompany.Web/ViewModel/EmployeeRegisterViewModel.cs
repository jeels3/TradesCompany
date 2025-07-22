using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.ViewModel
{
    public class EmployeeRegisterViewModel
    {
        [Required(ErrorMessage = "User Name is Required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAvailable", controller: "Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; } = "Employee"; // Default role for employees
        [Required(ErrorMessage = "Service Type is Required")]
        public int ServiceTypeId { get; set; }
        public List<ServiceType>? ServiceTypes { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
