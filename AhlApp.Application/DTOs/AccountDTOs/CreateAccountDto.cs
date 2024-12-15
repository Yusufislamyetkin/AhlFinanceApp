namespace AhlApp.Application.DTOs
{
    public class CreateAccountDto
    {
        public Guid UserId { get; set; }
        public decimal InitialBalance { get; set; }
        public string Currency { get; set; } // TRY, USD, EUR gibi
        public string AccountNumber { get; set; }
    }
}
