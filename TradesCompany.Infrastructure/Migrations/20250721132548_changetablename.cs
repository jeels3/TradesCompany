using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changetablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotations_ServiceMen_ServiceManId",
                table: "Quotations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceMen_AspNetUsers_UserId",
                table: "ServiceMen");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceMen_serviceTypes_ServiceTypeId",
                table: "ServiceMen");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_ServiceMen_ServiceManId",
                table: "ServiceSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceMen",
                table: "ServiceMen");

            migrationBuilder.RenameTable(
                name: "ServiceMen",
                newName: "ServiceMan");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceMen_UserId",
                table: "ServiceMan",
                newName: "IX_ServiceMan_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceMen_ServiceTypeId",
                table: "ServiceMan",
                newName: "IX_ServiceMan_ServiceTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceMan",
                table: "ServiceMan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotations_ServiceMan_ServiceManId",
                table: "Quotations",
                column: "ServiceManId",
                principalTable: "ServiceMan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceMan_AspNetUsers_UserId",
                table: "ServiceMan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceMan_serviceTypes_ServiceTypeId",
                table: "ServiceMan",
                column: "ServiceTypeId",
                principalTable: "serviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_ServiceMan_ServiceManId",
                table: "ServiceSchedules",
                column: "ServiceManId",
                principalTable: "ServiceMan",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotations_ServiceMan_ServiceManId",
                table: "Quotations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceMan_AspNetUsers_UserId",
                table: "ServiceMan");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceMan_serviceTypes_ServiceTypeId",
                table: "ServiceMan");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_ServiceMan_ServiceManId",
                table: "ServiceSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceMan",
                table: "ServiceMan");

            migrationBuilder.RenameTable(
                name: "ServiceMan",
                newName: "ServiceMen");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceMan_UserId",
                table: "ServiceMen",
                newName: "IX_ServiceMen_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceMan_ServiceTypeId",
                table: "ServiceMen",
                newName: "IX_ServiceMen_ServiceTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceMen",
                table: "ServiceMen",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotations_ServiceMen_ServiceManId",
                table: "Quotations",
                column: "ServiceManId",
                principalTable: "ServiceMen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceMen_AspNetUsers_UserId",
                table: "ServiceMen",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceMen_serviceTypes_ServiceTypeId",
                table: "ServiceMen",
                column: "ServiceTypeId",
                principalTable: "serviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_ServiceMen_ServiceManId",
                table: "ServiceSchedules",
                column: "ServiceManId",
                principalTable: "ServiceMen",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
