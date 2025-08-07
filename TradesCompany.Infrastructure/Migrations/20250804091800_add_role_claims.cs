using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_role_claims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "AspNetRoleClaims",
               columns: new[] { "Id", "RoleId", "ClaimType", "ClaimValue" },
               values: new object[,]
               {
                    { "1","1", "Booking Service", "Booking Service" },
                    { "2","2", "Send Quotation", "Send Quotation" },
                    { "3","2", "Schedule Service", "Schedule Service" }
               }
           );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
