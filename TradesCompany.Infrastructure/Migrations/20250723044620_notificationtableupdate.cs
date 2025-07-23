using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class notificationtableupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceManEmail",
                table: "ScheduleServiceByUser");

            migrationBuilder.RenameColumn(
                name: "ScheduleAt",
                table: "ScheduleServiceByUser",
                newName: "ScheduledAt");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleServiceId",
                table: "ScheduleServiceByUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "QuotationByUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "QuotationByServicerMan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingByServiceType",
                columns: table => new
                {
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bookingId = table.Column<int>(type: "int", nullable: true),
                    img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_userId",
                table: "Notification",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingByServiceType");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropColumn(
                name: "ScheduleServiceId",
                table: "ScheduleServiceByUser");

            migrationBuilder.DropColumn(
                name: "status",
                table: "QuotationByUser");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "QuotationByServicerMan");

            migrationBuilder.RenameColumn(
                name: "ScheduledAt",
                table: "ScheduleServiceByUser",
                newName: "ScheduleAt");

            migrationBuilder.AddColumn<string>(
                name: "ServiceManEmail",
                table: "ScheduleServiceByUser",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
