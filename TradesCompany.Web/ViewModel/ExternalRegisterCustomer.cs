using System.ComponentModel.DataAnnotations;

namespace TradesCompany.Web.ViewModel
{
    public class ExternalRegisterCustomer
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string ServiceType { get; set; }

        public string Role { get; set; }
    }
}
