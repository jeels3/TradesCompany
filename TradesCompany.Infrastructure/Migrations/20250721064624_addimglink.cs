using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addimglink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgLink",
                table: "serviceTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgLink",
                table: "serviceTypes");
        }
    }
}
