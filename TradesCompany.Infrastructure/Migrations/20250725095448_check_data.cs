using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class check_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "ScheduleServiceByUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ScheduleServiceByEmployee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Gst",
                table: "Bills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PlatFormFees",
                table: "Bills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "serviceCharge",
                table: "Bills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "ScheduleServiceByUser");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScheduleServiceByEmployee");

            migrationBuilder.DropColumn(
                name: "Gst",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PlatFormFees",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "serviceCharge",
                table: "Bills");
        }
    }
}
