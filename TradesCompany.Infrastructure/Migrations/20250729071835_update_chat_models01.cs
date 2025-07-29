using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_chat_models01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IsSeen_ChannelMessage_ChannelMessageId",
                table: "IsSeen");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "IsSeen",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelMessageId",
                table: "IsSeen",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IsSeen_ChannelMessage_ChannelMessageId",
                table: "IsSeen",
                column: "ChannelMessageId",
                principalTable: "ChannelMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IsSeen_ChannelMessage_ChannelMessageId",
                table: "IsSeen");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "IsSeen",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelMessageId",
                table: "IsSeen",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_IsSeen_ChannelMessage_ChannelMessageId",
                table: "IsSeen",
                column: "ChannelMessageId",
                principalTable: "ChannelMessage",
                principalColumn: "Id");
        }
    }
}
