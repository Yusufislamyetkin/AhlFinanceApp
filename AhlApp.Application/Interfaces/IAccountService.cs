using AhlApp.Application.DTOs;
using AhlApp.Shared.Models;

public interface IAccountService
{
    Task<Response<List<AccountResponseDto>>> GetAccountsByUserIdAsync(Guid userId);
}