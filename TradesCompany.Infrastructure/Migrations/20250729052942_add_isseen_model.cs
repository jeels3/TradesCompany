using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_isseen_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsSeenId",
                table: "ChannelMessage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IsSeen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Seen = table.Column<bool>(type: "bit", nullable: false),
                    SeenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsSeen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IsSeen_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessage_IsSeenId",
                table: "ChannelMessage",
                column: "IsSeenId");

            migrationBuilder.CreateIndex(
                name: "IX_IsSeen_UserId",
                table: "IsSeen",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessage_IsSeen_IsSeenId",
                table: "ChannelMessage",
                column: "IsSeenId",
                principalTable: "IsSeen",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_IsSeen_IsSeenId",
                table: "ChannelMessage");

            migrationBuilder.DropTable(
                name: "IsSeen");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMessage_IsSeenId",
                table: "ChannelMessage");

            migrationBuilder.DropColumn(
                name: "IsSeenId",
                table: "ChannelMessage");
        }
    }
}
