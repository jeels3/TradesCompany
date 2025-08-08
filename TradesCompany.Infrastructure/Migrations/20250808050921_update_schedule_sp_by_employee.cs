using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_schedule_sp_by_employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[GetAllScheduleServiceByEmployee]
                @userId Varchar(100)
                AS
                BEGIN
	                SELECT b.WorkDetails ,ss.Status ,  ss.Id as ScheduleServiceId,u.Id as CustomerUserId ,  u.UserName as CustomerName ,u.Email as CustomerEmail , su.UserName as ServiceMan , su.Id as ServiceManUserId ,st.ServiceName , ss.ScheduledAt , ss.TotalPrice
	                from ServiceSchedules ss 
	                inner join Bookings b on b.Id = ss.BookingId
	                inner join AspNetUsers u on u.Id = b.UserId
	                inner join ServiceMan sm on sm.Id = ss.ServiceManId
	                inner join serviceTypes st on st.Id = sm.ServiceTypeId
	                inner join AspNetUsers su on su.Id  = sm.UserId
	                where su.Id = @userId
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
