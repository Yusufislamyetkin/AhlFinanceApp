using AhlApp.Shared.Models;

namespace AhlApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Response<string>> AuthenticateAsync(string email, string password);
    }
}
