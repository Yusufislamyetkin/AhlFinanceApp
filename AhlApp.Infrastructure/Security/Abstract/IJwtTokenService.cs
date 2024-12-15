namespace AhlApp.Infrastructure.Security.Abstract
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string role);
        bool ValidateToken(string token);
    }
}
