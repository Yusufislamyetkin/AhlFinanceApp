using AhlApp.Application.DTOs;
using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlApp.Application.Interfaces
{
    public interface ITransactionReportService
    {
        Task<Response<List<TransactionAnalysisDto>>> GetTransactionsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<Response<List<MonthlySummaryDto>>> GetMonthlySummaryAsync(Guid userId);
        Task<Response<SpendingForecastDto>> GetSpendingForecastAsync(Guid userId);
        Task<Response<object>> GetTransactionSummaryAsync(Guid userId, int dateRangeType, int forecastType);

    }
}
