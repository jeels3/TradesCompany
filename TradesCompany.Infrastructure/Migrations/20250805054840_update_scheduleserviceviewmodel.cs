using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_scheduleserviceviewmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuotationDescription",
                table: "ScheduleServiceByEmployee");

            migrationBuilder.AddColumn<string>(
                name: "QuotationDescription",
                table: "QuotationByServicerMan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuotationDescription",
                table: "QuotationByServicerMan");

            migrationBuilder.AddColumn<string>(
                name: "QuotationDescription",
                table: "ScheduleServiceByEmployee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
