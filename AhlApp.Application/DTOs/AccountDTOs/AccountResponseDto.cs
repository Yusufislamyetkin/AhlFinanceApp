namespace AhlApp.Application.DTOs
{
    public class AccountResponseDto
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string AccountName { get; set; }
    }
}
