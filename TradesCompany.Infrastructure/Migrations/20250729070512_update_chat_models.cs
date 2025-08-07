using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_chat_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_IsSeen_IsSeenId",
                table: "ChannelMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelUser",
                table: "ChannelUser");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMessage_IsSeenId",
                table: "ChannelMessage");

            migrationBuilder.DropColumn(
                name: "ChannelName",
                table: "ChannelUser");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ChannelMessage");

            migrationBuilder.AddColumn<int>(
                name: "ChannelMessageId",
                table: "IsSeen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ChannelUser",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "ChannelMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelUser",
                table: "ChannelUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_IsSeen_ChannelMessageId",
                table: "IsSeen",
                column: "ChannelMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUser_UserId",
                table: "ChannelUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessage_ChannelId",
                table: "ChannelMessage",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_Channel_ChannelId",
                table: "ChannelMessage",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IsSeen_ChannelMessage_ChannelMessageId",
                table: "IsSeen",
                column: "ChannelMessageId",
                principalTable: "ChannelMessage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_Channel_ChannelId",
                table: "ChannelMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_IsSeen_ChannelMessage_ChannelMessageId",
                table: "IsSeen");

            migrationBuilder.DropIndex(
                name: "IX_IsSeen_ChannelMessageId",
                table: "IsSeen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelUser",
                table: "ChannelUser");

            migrationBuilder.DropIndex(
                name: "IX_ChannelUser_UserId",
                table: "ChannelUser");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMessage_ChannelId",
                table: "ChannelMessage");

            migrationBuilder.DropColumn(
                name: "ChannelMessageId",
                table: "IsSeen");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChannelUser");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "ChannelMessage");

            migrationBuilder.AddColumn<string>(
                name: "ChannelName",
                table: "ChannelUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ChannelMessage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelUser",
                table: "ChannelUser",
                columns: new[] { "UserId", "ChannelName" });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessage_IsSeenId",
                table: "ChannelMessage",
                column: "IsSeenId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_IsSeen_IsSeenId",
                table: "ChannelMessage",
                column: "IsSeenId",
                principalTable: "IsSeen",
                principalColumn: "Id");
        }
    }
}
