using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_channelmessage_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_AspNetUsers_UserId",
                table: "ChannelMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMessage_UserId",
                table: "ChannelMessage");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChannelMessage");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "ChannelMessage",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessage_SenderId",
                table: "ChannelMessage",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_AspNetUsers_SenderId",
                table: "ChannelMessage",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_AspNetUsers_SenderId",
                table: "ChannelMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMessage_SenderId",
                table: "ChannelMessage");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "ChannelMessage",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChannelMessage",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessage_UserId",
                table: "ChannelMessage",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_AspNetUsers_UserId",
                table: "ChannelMessage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
