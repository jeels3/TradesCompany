using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure;
using TradesCompany.Infrastructure.Data;
using TradesCompany.Shared.Hubs;
using TradesCompany.Web.Middlewares;

namespace TradesCompany.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; // Configuration is help to access appsettings.json and other configuration sources
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC services
            services.AddControllersWithViews();

            var connectionString = Configuration.GetConnectionString("SQLServerIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
            // Add Identity services
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            // Add Google Services
            var GoogleClientId = Configuration["Google:AppId"];
            var GoogleClientSecret = Configuration["Google:AppSecret"];
            services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = GoogleClientId;
                options.ClientSecret = GoogleClientSecret;
            });

            // add policy 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role"));
                options.AddPolicy("BookingServicePolicy", policy => policy.RequireClaim("Booking Service"));
                options.AddPolicy("SendQuotationPolicy", policy => policy.RequireClaim("Send Quotation"));
                options.AddPolicy("ScheduleServicePolicy", policy => policy.RequireClaim("Schedule Service"));
                options.AddPolicy("CancelSchedulePolicy", policy => policy.RequireClaim("Cancel Schedule"));
            });

            // Register custom services
            services.AddInfrastructure();
            services.AddSignalR();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddExceptionHandler<BadRequestExceptionHandler>();

            

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error"); // Or a custom path/middleware
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapHub<NotificationHub>("/chatHub");
            });
        }
    }
}
