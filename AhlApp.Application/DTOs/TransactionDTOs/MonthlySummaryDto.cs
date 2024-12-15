namespace AhlApp.Application.DTOs
{
    public class MonthlySummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public List<AccountAnalysisDto> Accounts { get; set; } = new();
    }

    public class AccountAnalysisDto
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public List<CategoryAnalysisDto> IncomeCategoryDetails { get; set; } = new();
        public List<CategoryAnalysisDto> ExpenseCategoryDetails { get; set; } = new();
    }

    public class CategoryAnalysisDto
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
    }
}
