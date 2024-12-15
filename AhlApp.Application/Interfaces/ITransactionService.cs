using AhlApp.Application.DTOs;
using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<Response<Guid>> CreateExpenseAsync(Guid userId, CreateExpenseDto dto);
        Task<Response<Guid>> DepositFundsAsync(Guid userId, DepositFundsDto dto);
        Task<Response<Guid>> TransferFundsAsync(Guid userId, TransferFundsDto dto);
    }
}
