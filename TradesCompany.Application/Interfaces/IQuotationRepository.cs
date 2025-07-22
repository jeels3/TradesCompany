using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Application.Interfaces
{
    public interface IQuotationRepository
    {
        Task<List<QuotationByUser>> GetQuotationForUser(string userId);
    }
}
