using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Application.DTOs;
using AhlApp.Application.Interfaces;
using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using AhlApp.Domain.Constants;

public class TransactionReportService : ITransactionReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<List<TransactionAnalysisDto>>> GetTransactionsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _unitOfWork.Transactions.GetByUserAndDateRangeAsync(userId, startDate, endDate);

        if (transactions == null || !transactions.Any())
        {
            return Response<List<TransactionAnalysisDto>>.ErrorResponse(ErrorMessages.NoTransactionsFoundForDateRange);
        }

        var result = transactions.Select(t => new TransactionAnalysisDto
        {
            Id = t.Id,
            Amount = t.Amount.Amount,
            Description = t.Description,
            Category = t.Category.Name,
            Date = t.TransactionDate
        }).ToList();

        return Response<List<TransactionAnalysisDto>>.SuccessResponse(result);
    }

    public async Task<Response<List<MonthlySummaryDto>>> GetMonthlySummaryAsync(Guid userId)
    {
        var userAccounts = await _unitOfWork.Accounts.GetAccountsByUserIdAsync(userId);
        if (userAccounts == null || !userAccounts.Any())
        {
            return Response<List<MonthlySummaryDto>>.ErrorResponse(ErrorMessages.NoAccountsFoundForUser);
        }

        var transactions = await _unitOfWork.Transactions.GetByUserAsync(userId);
        var userAccountIds = userAccounts.Select(a => a.Id).ToHashSet();

        var groupedByMonth = transactions.GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month });

        var monthlySummaries = groupedByMonth.Select(monthGroup =>
        {
            var incomeTransactions = monthGroup.Where(t =>
                (t.TransactionType == TransactionType.Deposit && userAccountIds.Contains(t.AccountId.Value)) ||
                (t.TransactionType == TransactionType.Transfer && userAccountIds.Contains(t.ReceiverAccountId.Value)));

            var expenseTransactions = monthGroup.Where(t =>
                (t.TransactionType == TransactionType.Expense && userAccountIds.Contains(t.AccountId.Value)) ||
                (t.TransactionType == TransactionType.Transfer && userAccountIds.Contains(t.SenderAccountId.Value)));

            var incomeCategoryDetails = incomeTransactions
                .GroupBy(t => t.Category?.Name ?? "Unknown")
                .Select(categoryGroup => new CategoryAnalysisDto
                {
                    CategoryName = categoryGroup.Key,
                    TotalAmount = categoryGroup.Sum(t => t.Amount.Amount),
                    Percentage = incomeTransactions.Sum(i => i.Amount.Amount) == 0
                        ? 0
                        : (categoryGroup.Sum(t => t.Amount.Amount) / incomeTransactions.Sum(i => i.Amount.Amount)) * 100
                }).ToList();

            var expenseCategoryDetails = expenseTransactions
                .GroupBy(t => t.Category?.Name ?? "Unknown")
                .Select(categoryGroup => new CategoryAnalysisDto
                {
                    CategoryName = categoryGroup.Key,
                    TotalAmount = categoryGroup.Sum(t => t.Amount.Amount),
                    Percentage = expenseTransactions.Sum(e => e.Amount.Amount) == 0
                        ? 0
                        : (categoryGroup.Sum(t => t.Amount.Amount) / expenseTransactions.Sum(e => e.Amount.Amount)) * 100
                }).ToList();

            return new MonthlySummaryDto
            {
                Year = monthGroup.Key.Year,
                Month = monthGroup.Key.Month,
                Accounts = userAccounts.Select(a => new AccountAnalysisDto
                {
                    AccountId = a.Id,
                    AccountName = a.AccountName,
                    TotalIncome = incomeTransactions.Where(t => t.AccountId == a.Id || t.ReceiverAccountId == a.Id).Sum(t => t.Amount.Amount),
                    TotalExpense = expenseTransactions.Where(t => t.AccountId == a.Id || t.SenderAccountId == a.Id).Sum(t => t.Amount.Amount),
                    IncomeCategoryDetails = incomeCategoryDetails,
                    ExpenseCategoryDetails = expenseCategoryDetails
                }).ToList()
            };
        }).ToList();

        return Response<List<MonthlySummaryDto>>.SuccessResponse(monthlySummaries);
    }

    public async Task<Response<SpendingForecastDto>> GetSpendingForecastAsync(Guid userId)
    {
        var transactions = await _unitOfWork.Transactions.GetByUserAsync(userId);

        if (transactions == null || !transactions.Any())
        {
            return Response<SpendingForecastDto>.ErrorResponse(ErrorMessages.NoTransactionsFoundForForecast);
        }

        var averageExpense = transactions.Where(t => t.TransactionType == TransactionType.Expense)
                                         .Average(t => t.Amount.Amount);

        var averageIncome = transactions.Where(t => t.TransactionType == TransactionType.Deposit)
                                        .Average(t => t.Amount.Amount);

        var forecast = new SpendingForecastDto
        {
            PredictedExpense = averageExpense,
            PredictedIncome = averageIncome
        };

        return Response<SpendingForecastDto>.SuccessResponse(forecast);
    }

    public async Task<Response<object>> GetTransactionSummaryAsync(Guid userId, int dateRangeType, int forecastType)
    {
        var userAccounts = await _unitOfWork.Accounts.GetAccountsByUserIdAsync(userId);
        if (userAccounts == null || !userAccounts.Any())
            return Response<object>.ErrorResponse(ErrorMessages.NoAccountsFoundForUser);

        var transactions = await _unitOfWork.Transactions.GetByUserAsync(userId);
        var now = DateTime.UtcNow;

        DateTime startDate = dateRangeType switch
        {
            0 => now.Date,
            1 => now.AddDays(-7),
            2 => new DateTime(now.Year, now.Month, 1),
            3 => new DateTime(now.Year, 1, 1),
            4 => DateTime.MinValue,
            _ => throw new ArgumentException(ErrorMessages.InvalidDateRangeType)
        };

        var filteredTransactions = transactions.Where(t => t.TransactionDate >= startDate || dateRangeType == 4).ToList();
        if (!filteredTransactions.Any())
            return Response<object>.ErrorResponse(ErrorMessages.NoTransactionsFoundForDateRange);

        var accountAnalysisDtos = userAccounts.Select(account =>
        {
            var incomeTransactions = filteredTransactions.Where(t =>
                (t.TransactionType == TransactionType.Deposit && t.AccountId == account.Id) ||
                (t.TransactionType == TransactionType.Transfer && t.ReceiverAccountId == account.Id));

            var expenseTransactions = filteredTransactions.Where(t =>
                (t.TransactionType == TransactionType.Expense && t.AccountId == account.Id) ||
                (t.TransactionType == TransactionType.Transfer && t.SenderAccountId == account.Id));

            return new AccountAnalysisDto
            {
                AccountId = account.Id,
                AccountName = account.AccountName,
                TotalIncome = incomeTransactions.Sum(t => t.Amount.Amount),
                TotalExpense = expenseTransactions.Sum(t => t.Amount.Amount),
                IncomeCategoryDetails = incomeTransactions
                    .GroupBy(t => t.Category?.Name ?? ErrorMessages.InvalidCategory)
                    .Select(group => new CategoryAnalysisDto
                    {
                        CategoryName = group.Key,
                        TotalAmount = group.Sum(t => t.Amount.Amount),
                        Percentage = group.Sum(t => t.Amount.Amount) /
                                     incomeTransactions.Sum(i => i.Amount.Amount) * 100
                    }).ToList(),
                ExpenseCategoryDetails = expenseTransactions
                    .GroupBy(t => t.Category?.Name ?? ErrorMessages.InvalidCategory)
                    .Select(group => new CategoryAnalysisDto
                    {
                        CategoryName = group.Key,
                        TotalAmount = group.Sum(t => t.Amount.Amount),
                        Percentage = group.Sum(t => t.Amount.Amount) /
                                     expenseTransactions.Sum(e => e.Amount.Amount) * 100
                    }).ToList()
            };
        }).ToList();

        if (forecastType == 1)
        {
            var analysis = GenerateForecastAnalysis(filteredTransactions, accountAnalysisDtos);
            return Response<object>.SuccessResponse(analysis);
        }

        return Response<object>.SuccessResponse(new { Accounts = accountAnalysisDtos });
    }


    private object GenerateForecastAnalysis(List<Transaction> transactions, List<AccountAnalysisDto> accountAnalysisDtos)
    {
        if (transactions == null || !transactions.Any())
            throw new InvalidOperationException(ErrorMessages.NoTransactionsFoundForForecast);

        var totalIncome = accountAnalysisDtos.Sum(a => a.TotalIncome);
        var totalExpense = accountAnalysisDtos.Sum(a => a.TotalExpense);
        var balance = Math.Max(totalIncome - totalExpense, 0);

        // Harcama ve gelir kategorilerini gruplama
        var expenseCategories = accountAnalysisDtos.SelectMany(a => a.ExpenseCategoryDetails)
            .GroupBy(c => c.CategoryName)
            .Select(group => new
            {
                Category = group.Key,
                TotalExpense = group.Sum(c => c.TotalAmount),
                Percentage = (group.Sum(c => c.TotalAmount) / totalExpense) * 100
            })
            .OrderByDescending(c => c.TotalExpense)
            .ToList();

        var incomeCategories = accountAnalysisDtos.SelectMany(a => a.IncomeCategoryDetails)
            .GroupBy(c => c.CategoryName)
            .Select(group => new
            {
                Category = group.Key,
                TotalIncome = group.Sum(c => c.TotalAmount),
                Percentage = (group.Sum(c => c.TotalAmount) / totalIncome) * 100
            })
            .OrderByDescending(c => c.TotalIncome)
            .ToList();

        // JSON formatlı detaylı rapor
        return new
        {
            Summary = new
            {
                TotalIncome = $"Toplam Gelir: {totalIncome:C2}",
                TotalExpense = $"Toplam Gider: {totalExpense:C2}",
                NetBalance = $"Net Bakiye: {balance:C2}"
            },
            ExpenseAnalysis = expenseCategories.Select(c =>
                $"{c.Category} kategorisinde toplam {c.TotalExpense:C2} harcama yapmışsınız. Bu, toplam harcamalarınızın %{c.Percentage:F2}'sine denk geliyor.").ToList(),
            IncomeAnalysis = incomeCategories.Select(c =>
                $"{c.Category} kategorisinden toplam {c.TotalIncome:C2} gelir elde etmişsiniz. Bu, toplam gelirinizin %{c.Percentage:F2}'sine denk geliyor.").ToList(),
            TopExpenseCategories = expenseCategories.Select((c, index) =>
                $"{index + 1}. {c.Category} - {c.TotalExpense:C2} (Toplam Harcama)").ToList(),
            TopIncomeCategories = incomeCategories.Select((c, index) =>
                $"{index + 1}. {c.Category} - {c.TotalIncome:C2} (Toplam Gelir)").ToList(),
            Recommendations = new
            {
                Expense = expenseCategories.Any()
                    ? $"Harcama alışkanlıklarınıza dikkat edin. {expenseCategories.First().Category} kategorisindeki harcamalarınızı azaltmayı düşünebilirsiniz."
                    : "Harcama kategorileriniz dengeli görünüyor.",
                Income = incomeCategories.Any()
                    ? $"Gelir kaynaklarınızı artırmak için yeni fırsatlar değerlendirin. {incomeCategories.First().Category} kategorisinde daha fazla gelir fırsatları araştırabilirsiniz."
                    : "Gelir kaynaklarınızı çeşitlendirmeyi düşünebilirsiniz."
            },
            Note = "Bu rapor, bütçenizi daha etkili yönetmenize ve gelecekteki mali hedeflerinize ulaşmanıza yardımcı olabilir."
        };
    }







}
