using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addquotationbyuserinservicemanemail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleServiceByUser",
                columns: table => new
                {
                    WorkDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceMan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceManUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceManEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleServiceByUser");
        }
    }
}
