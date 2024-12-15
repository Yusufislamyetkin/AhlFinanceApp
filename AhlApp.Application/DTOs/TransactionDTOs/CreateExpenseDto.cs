namespace AhlApp.Application.DTOs
{
    public class CreateExpenseDto
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
    }
}
