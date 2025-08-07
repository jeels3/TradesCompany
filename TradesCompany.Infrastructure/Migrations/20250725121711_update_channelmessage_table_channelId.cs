using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_channelmessage_table_channelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessage_Channel_ChannelId",
                table: "ChannelMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMessage_ChannelId",
                table: "ChannelMessage");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "ChannelMessage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "ChannelMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
