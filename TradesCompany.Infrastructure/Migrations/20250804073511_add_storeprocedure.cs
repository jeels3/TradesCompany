using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_storeprocedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[GetAllBookingByServiceType]
                   @ServiceTypeId INT 
                AS
                BEGIN
                   SELECT u.UserName as CustomerName ,b.Price ,  u.Email , b.Status , b.CreatedAt , b.WorkDetails , b.Id as bookingID , b.imagepath as img
                   FROM Bookings b 
                   inner join AspNetUsers u on u.Id = b.UserId
                   where b.ServiceTypeId = @ServiceTypeId
                END
            ");

            migrationBuilder.Sql(@"
               CREATE OR ALTER PROCEDURE [dbo].[GetAllMessageByChannelNameForGroup]
                   @ChannelName VARCHAR(100)
                AS
                BEGIN
                   select * from 
                   ChannelMessage cm 
                   left join AspNetUsers u on cm.SenderId = u.Id
                   left join Channel c on cm.ChannelId = c.Id
                   left join IsSeen s on s.ChannelMessageId = cm.Id
                   where cm.ChannelName = @ChannelName
                   order by cm.CreateAt
                END
            ");

            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetAllQuotationByEmployee]
                @userId varchar(100)
                AS
                BEGIN
                    SELECT u.UserName as CustomerName ,q.Status , u.Id as customerId, q.Id as quotationId, b.WorkDetails , q.Price , st.ServiceName , su.UserName as ServicemanName , su.Id as userId
                    FROM Quotations q
                    left join Bookings b on b.Id = q.BookingId
                    left join ServiceMan sm on sm.Id = q.ServiceManId
                    left join serviceTypes st on st.Id = sm.ServiceTypeId
                    left join AspNetUsers su on su.Id = sm.UserId
                    left join AspNetUsers u on b.UserId = u.Id
                    where sm.UserId = @userId AND (q.Status = 'Accepted' or q.Status = 'Pending') 
                END
            ");

            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetAllQuotationForUser]
               @userId varchar(100)
                AS
                BEGIN
                   SELECT u.UserName ,q.QuotationDescription as Description , q.Id as quotationId,su.Email as ServiceManEmail,q.Status as status, b.WorkDetails , q.Price , st.ServiceName , su.UserName as ServicemanName , su.Id as userId 
                   FROM Quotations  q
                   left join Bookings b on q.BookingId = b.Id
                   left join AspNetUsers u on b.UserId = u.Id
                   left join ServiceMan sm on sm.Id = q.ServiceManId
                   left join serviceTypes st on st.Id = sm.ServiceTypeId
                   left join AspNetUsers su on su.Id = sm.UserId
                   where u.Id = @userId
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[GetAllRole]
                AS
                BEGIN
                    SELECT *
                    FROM AspNetRoles;
                END;
            ");

            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetAllScheduleServiceByEmployee]
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
	                where su.Id = @userId AND (ss.Status != 'Completed' AND ss. Status != 'Cancelled')
                END
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetAllScheduleServiceByUser]
	            @userId Varchar(100)
                AS
                BEGIN
	                SELECT b.WorkDetails ,ss.Status ,  ss.Id as ScheduleServiceId, su.UserName as ServiceMan , su.Id as ServiceManUserId ,st.ServiceName , ss.ScheduledAt , ss.TotalPrice
	                from ServiceSchedules ss 
	                inner join Bookings b on b.Id = ss.BookingId
	                inner join AspNetUsers u on u.Id = b.UserId
	                inner join ServiceMan sm on sm.Id = ss.ServiceManId
	                inner join serviceTypes st on st.Id = sm.ServiceTypeId
	                inner join AspNetUsers su on su.Id  = sm.UserId
	                where u.Id = @userId 
                END
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetAllServicemanByServiceType]
                @ServiceTypeId INT
                AS
                BEGIN
                    SELECT sm.Id , st.ServiceName ,u.Id as userId ,  u.UserName , u.Email , AVG(r.Stars) as ratings , r.Feedback
                    FROM ServiceMan sm
                    left join  serviceTypes st on st.Id = sm.ServiceTypeId
                    left join AspNetUsers u on sm.UserId = u.Id
                    left join ServiceSchedules ss on ss.ServiceManId = sm.Id
                    left join Ratings r on r.ServiceScheduleId = ss.Id
                    where sm.ServiceTypeId = @ServiceTypeId
                    group by sm.Id , st.ServiceName , u.Id , u.UserName , u.Email , r.Feedback
                END
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER PROCEDURE [dbo].[GetAllUser]
                AS
                BEGIN
                    SELECT *
                    FROM AspNetUsers;
                END;
            ");

            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetAllUsersWithRole]
                AS
                BEGIN
                    SELECT u.Id as userId ,u.IsBlocked , u.UserName ,r.Id as roleId , r.Name as RoleName , u.Email 
                    from AspNetUsers u 
                    left join AspNetUserRoles ur
                    on u.Id = ur.UserId
                    left join AspNetRoles r 
                    on ur.RoleId = r.Id
                    order by u.UserName;
                END
            ");

            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetRevenueVSServiceType]
                AS
                BEGIN
	                select sum(ss.TotalPrice) as revenue , st.ServiceName
	                from ServiceSchedules ss 
	                left join ServiceMan sm on ss.ServiceManId = sm.Id
	                left join serviceTypes st on st.Id = sm.ServiceTypeId
	                group by st.ServiceName
                END
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetRevenueVSServiceTypeByMonth]
                AS
                BEGIN
	                select sum(ss.TotalPrice) as revenue , MONTH(ss.ScheduledAt) as months
	                from ServiceSchedules ss 
	                left join ServiceMan sm on ss.ServiceManId = sm.Id
	                left join serviceTypes st on st.Id = sm.ServiceTypeId
	                group by MONTH(ss.ScheduledAt)
                END
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetRevenueVSServiceTypeByYear]
                AS
                BEGIN
	                select sum(ss.TotalPrice) as revenue , st.ServiceName , Year(ss.ScheduledAt) as months
	                from ServiceSchedules ss 
	                left join ServiceMan sm on ss.ServiceManId = sm.Id
	                left join serviceTypes st on st.Id = sm.ServiceTypeId
	                group by st.ServiceName , Year(ss.ScheduledAt)
                END
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[GetUnreadMessageCountByUserId]
               @userId INT
                AS
                BEGIN
                   select * from 
                   AspNetUsers u 
                   left join ChannelUser cu on cu.UserId = u.Id
                   -- left join Channel c on c.id = cu.ChannelId
                   left join ChannelMessage cm on cm.ChannelId = cu.ChannelId
                   left join IsSeen s on s.ChannelMessageId = cm.Id
                END
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[SelectAllCustomers]
                AS
                select * from AspNetUsers 
                left join Bookings
                on AspNetUsers.Id = Bookings.UserId 
                left join AspNetUserRoles
                on AspNetUsers.Id = AspNetUserRoles.RoleId
                left join AspNetRoles
                on AspNetUserRoles.RoleId = AspNetRoles.id
                order by Bookings.CreatedAt;
            ");
            migrationBuilder.Sql(@"
               CREATE OR ALTER   PROCEDURE [dbo].[SelectAllUsers]
                AS
                select * from AspNetUsers as u
                left join AspNetUserRoles as ur
                on u.Id = ur.RoleId
                left join AspNetRoles as r
                on ur.RoleId = r.id
            ");

            // Insert Data 
            migrationBuilder.InsertData(
                table: "serviceTypes", // Replace with your table name
                columns: new[] { "ServiceName", "ImgLink", }, // Replace with your column names
                values: new object[,]
                {
                    { "Electrition", "https://lh3.googleusercontent.com/aida-public/AB6AXuCbr4AGd4fcQOMTA2rQzd7ScTI411pq3R56aBDNlYnOj2AEnIdRCjOdguc4eQPj6ZTCOLkNl0lr1Rjx6mzo0U2dl2FVUGi7MmO9_0AX-yOoMSwBi812YI5PzVN2ljbB9SdAOiPtyjF95F8sjncctUB2VAVnukhW7PEQkMgUGEF0r5mFlTpZQS2wYnIm-DZ4l4rsWLpP4Z230aVYrQo3HoC7pwZp_13ZgVOtg7XWyfWpYlhkeubp1qqkw4HD2iWMUs9FctbVBEIw58w" },
                    { "Plumber", "https://lh3.googleusercontent.com/aida-public/AB6AXuC8d6LCW0wHjraP_KWPedmFV-9NJt_RpDSRktdGMdn_LHtdYVSUT_r5FY2MMcax3huwIkaE_DMBsOOZoOyHkMlkrlEjLnj8a5uG2ipOvYCUI0Lx8115mgCm-fxGGENF8kuiu2Rob3zu76OzaIWtEPCihXrCDu4ww6NCDd2iBBJpaa971EVGMMfubJS_zwzM5qmnBkM71vXFBH7GnvIt4mRz-PHAT6HkqurUZko2rU2wR5sGJJgQ2SBbgh9HFvFSICYiFi5egsrwpFQ" },
                    { "Cleaning", "https://lh3.googleusercontent.com/aida-public/AB6AXuAxgADFEYKOSPzeWV-3GyfiJwAfqAPKzoGFUsNrkyScw_c7cDQIfjJ6bpdtFBRom8M2byihS1q5XmWLVtb1yRLfn4bWyIVjPafH7VZrAyGOFpYz2Zv8gT_kh1gS57-N-4oyhOSMReMyKpKEdOh5XQ__1X_31BSfYpnsX90dkluse_M-5aWEjF306m7M9BhEFyYtY4KfqVjX1uBR78kh-w-5z1dx0dpvXRRSiGQpyvWW6aV66LjpJPvZ_TmRAhLvKy8mMxxxrPiMfzQ" },
                    { "Carpenter", "https://lh3.googleusercontent.com/aida-public/AB6AXuBMRJ_Sxri1SlWTp-b11zOZKEP1CvQ4A7hJeSxveUfdpBiXlLQRUmjO_MY2m-HcgpqdgE1UuRA2e8gqESIx85yhLXogd3ZncyD4Qt9NstZW8kI_sOaMk6W-qE_mTEwZ9RaZIgXj-volGaO91ZDKzSCOq_ezzmJR5r-4HtxDkgTJ3PVdTmV9YYH287eKyo-RLy7dCfzHVgHyAVxl9QDqlAUCXmvu7xxVeyOzBVRPiLLIIWloOVKLW71e34VEtvj9W0PwMF773uTN4Ew" },
                    { "Gardening", "https://lh3.googleusercontent.com/aida-public/AB6AXuBMRJ_Sxri1SlWTp-b11zOZKEP1CvQ4A7hJeSxveUfdpBiXlLQRUmjO_MY2m-HcgpqdgE1UuRA2e8gqESIx85yhLXogd3ZncyD4Qt9NstZW8kI_sOaMk6W-qE_mTEwZ9RaZIgXj-volGaO91ZDKzSCOq_ezzmJR5r-4HtxDkgTJ3PVdTmV9YYH287eKyo-RLy7dCfzHVgHyAVxl9QDqlAUCXmvu7xxVeyOzBVRPiLLIIWloOVKLW71e34VEtvj9W0PwMF773uTN4Ew" }
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "USER", "USER" },
                    { "2", "EMPLOYEE", "EMPLOYEE" },
                    { "3", "ADMIN", "ADMIN" }
                }
            );

            migrationBuilder.InsertData(
                    table: "AspNetUsers",
                    columns: new[] { "Id", "IsBlocked", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                    values: new object[] {
                        "0af8cbcf-8754-4a65-9879-8ca355c3eb18",
                        false,
                        "admin",
                        "ADMIN",
                        "admin@gmail.com",
                        "ADMIN@GMAIL.COM",
                        true,
                        "AQAAAAIAAYagAAAAEOOaFmn4rcHwlo9Ycvj2Xyp19PXDjQee8oUszIQgCP6k/f03QGyqPe0SCcImDWGKPA==", // Generate a hashed password (e.g., using UserManager.HashPassword)
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        false,
                        false,
                        false,
                        0
                    }
                );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "0af8cbcf-8754-4a65-9879-8ca355c3eb18", "3" } // "3" is the ADMIN role id
            );


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
