using System.ComponentModel.DataAnnotations;

namespace TradesCompany.Web.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
