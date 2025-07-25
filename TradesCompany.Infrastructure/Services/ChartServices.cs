using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Infrastructure.Data;

namespace TradesCompany.Infrastructure.Services
{
    public class ChartServices 
    {
        private readonly ApplicationDbContext _context;

        public ChartServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChartModel> GetChartData()
        {
            var result = await _context.ServiceSchedules
                                                        .Select(x => new ChartModel.DataPoint
                                                        {
                                                            Label = x.ServiceMan.ServiceTypes.ServiceName,
                                                            Value = (double)x.TotalPrice
                                                        })
                .ToListAsync();

            ChartModel model = new ChartModel
            {
                Data = result
            };
            return model;
        }

    }
}
