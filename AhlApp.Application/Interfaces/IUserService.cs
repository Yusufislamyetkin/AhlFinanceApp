using AhlApp.Application.DTOs;
using AhlApp.Application.DTOs.UserDTOs;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<Response<UserResponseDto>> RegisterUserAsync(string name, string email, string password);
        Task<UserResponseDto> GetUserByIdAsync(Guid userId);
    
    }
}
