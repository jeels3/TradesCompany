using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.ViewModel
{
    public class ExternalRegisterWorkerViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public List<ServiceType> ServiceType { get; set; }
        public int ServiceTypeId { get; set; }
        public string Role { get; set; }
    }
}
