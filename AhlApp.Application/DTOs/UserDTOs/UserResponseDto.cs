
namespace AhlApp.Application.DTOs
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<AccountResponseDto> Accounts { get; set; }
    }
}
