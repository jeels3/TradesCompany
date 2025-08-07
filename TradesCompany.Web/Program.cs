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
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}