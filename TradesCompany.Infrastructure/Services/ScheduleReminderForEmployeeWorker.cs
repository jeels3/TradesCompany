using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.Services;

namespace TradesCompany.Infrastructure.Services
{
    public class ScheduleReminderForEmployeeWorker : BackgroundService
    {
        private readonly ILogger<ScheduleReminderForEmployeeWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        public ScheduleReminderForEmployeeWorker(ILogger<ScheduleReminderForEmployeeWorker> logger,
                                                IServiceProvider serviceProvider
                                                )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ScheduleReminderForEmployeeWorker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var employeeServices = scope.ServiceProvider.GetRequiredService<IEmployeeServices>();
                    var notificationservice = scope.ServiceProvider.GetRequiredService<NotificationService>();
                    var data = await employeeServices.GetAllServiceManForSreviceNotification();
                    foreach ( var employee in data )
                    {
                        await notificationservice.SendNotificationOfScheduleServiceToEmployee(employee, "Service Reminder", "Your Service Is Schedule In Next 30 Minutes");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending reminders.");
                }

                // Wait for 24 hours before running again
                await Task.Delay(TimeSpan.FromMinutes(29), stoppingToken);
            }
            _logger.LogInformation("ScheduleReminderForEmployeeWorker stopping.");
        }
    }
}
