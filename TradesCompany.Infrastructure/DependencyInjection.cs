using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Infrastructure.Repository;
using TradesCompany.Infrastructure.Services;

namespace TradesCompany.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHostedService<ScheduleReminderForEmployeeWorker>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<IServiceManRepository, ServiceManRepositor>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddTransient<EmailService>();
            services.AddScoped<ChartServices>();
            services.AddScoped<ExcelService>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<ConnectionService>();
            //services.AddHostedService<DummyWorker>();
            return services;
        }
    }
}
