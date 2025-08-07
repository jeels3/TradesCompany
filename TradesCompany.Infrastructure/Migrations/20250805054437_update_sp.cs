using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Migrations;
using TradesCompany.Domain.Entities;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_sp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuotationDescription",
                table: "ScheduleServiceByEmployee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE[dbo].[GetAllQuotationByEmployee]
                @userId varchar(100)
                AS
                BEGIN
                    SELECT u.UserName as CustomerName ,q.QuotationDescription as QuotationDescription ,  q.Status , u.Id as customerId, q.Id as quotationId, b.WorkDetails , q.Price , st.ServiceName , su.UserName as ServicemanName , su.Id as userId
                    FROM Quotations q
                    left join Bookings b on b.Id = q.BookingId
                    left join ServiceMan sm on sm.Id = q.ServiceManId
                    left join serviceTypes st on st.Id = sm.ServiceTypeId
                    left join AspNetUsers su on su.Id = sm.UserId
                    left join AspNetUsers u on b.UserId = u.Id
                    where sm.UserId = @userId AND(q.Status = 'Accepted' or q.Status = 'Pending')
                END
            ");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuotationDescription",
                table: "ScheduleServiceByEmployee");
        }
    }
}
