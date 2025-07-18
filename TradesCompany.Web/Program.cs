
 using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradesCompany.Application.Interfaces;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure.Data;
using TradesCompany.Infrastructure.Repository;

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
                options.AddPolicy("BookingServicePolicy", policy => policy.RequireClaim(" Booking Service"));
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
