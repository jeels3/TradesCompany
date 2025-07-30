using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Infrastructure.Services;

namespace TradesCompany.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHostedService<ScheduleReminderForEmployeeWorker>();
            //services.AddHostedService<DummyWorker>();
            return services;
        }
    }
}
