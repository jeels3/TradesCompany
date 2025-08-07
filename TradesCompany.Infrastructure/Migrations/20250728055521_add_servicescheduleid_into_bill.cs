using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_servicescheduleid_into_bill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "serviceScheduleId",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "serviceScheduleId1",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_serviceScheduleId1",
                table: "Bills",
                column: "serviceScheduleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_ServiceSchedules_serviceScheduleId1",
                table: "Bills",
                column: "serviceScheduleId1",
                principalTable: "ServiceSchedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_ServiceSchedules_serviceScheduleId1",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_serviceScheduleId1",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "serviceScheduleId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "serviceScheduleId1",
                table: "Bills");
        }
    }
}
