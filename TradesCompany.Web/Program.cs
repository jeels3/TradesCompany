
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure;
using TradesCompany.Infrastructure.Data;
using TradesCompany.Infrastructure.Repository;
using TradesCompany.Infrastructure.Services;
using TradesCompany.Shared.Hubs;
using TradesCompany.Web.Middlewares;

namespace TradesCompany.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            var GoogleClientId = builder.Configuration["Google:AppId"];
            var GoogleClientSecret = builder.Configuration["Google:AppSecret"];

            builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = GoogleClientId;
                options.ClientSecret = GoogleClientSecret;
            });

            // add policy 
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role"));
                options.AddPolicy("BookingServicePolicy", policy => policy.RequireClaim("Booking Service"));
                options.AddPolicy("SendQuotationPolicy", policy => policy.RequireClaim("Send Quotation"));
                options.AddPolicy("ScheduleServicePolicy", policy => policy.RequireClaim("Schedule Service"));
                options.AddPolicy("CancelSchedulePolicy", policy => policy.RequireClaim("Cancel Schedule"));
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IQuotationRepository, QuotationRepository>();
            builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
            builder.Services.AddScoped<IServiceManRepository, ServiceManRepositor>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
            builder.Services.AddInfrastructure();
            builder.Services.AddSignalR();
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddScoped<ChartServices>();
            builder.Services.AddScoped<ExcelService>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<ConnectionService>();

            // Exception
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseExceptionHandler();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapHub<NotificationHub>("/notificationHub");
            app.MapHub<NotificationHub>("/chatHub");

            app.Run();
        }
    }
}