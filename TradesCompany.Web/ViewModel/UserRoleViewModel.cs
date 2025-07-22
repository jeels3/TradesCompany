using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.ViewModel
{
    public class UserRoleViewModel
    {
        // From ASPNETUSERS
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string CuncurrancyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool LockoutFailedCount { get; set; }
        public int AccessFailedCount { get; set; }

        // From ASPNETROLES
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string NormalizedRoleName { get; set; }
        public string RoleConcurrencyStamp { get; set; }
    }
}
 
