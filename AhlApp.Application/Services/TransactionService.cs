using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Application.DTOs;
using AhlApp.Application.Features.Transactions.Commands;
using AhlApp.Application.Interfaces;
using AhlApp.Domain.Constants;
using AhlApp.Shared.Models;
using MediatR; 

public class TransactionService : ITransactionService
{
    private readonly IMediator _mediator;
    private readonly IAccountService _accountService;

    public TransactionService(IMediator mediator, IAccountService accountService)
    {
        _mediator = mediator;
        _accountService = accountService;
    }

    public async Task<Response<Guid>> CreateExpenseAsync(Guid userId, CreateExpenseDto dto)
    {
        var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
        if (!accounts.Data.Any(a => a.AccountId == dto.AccountId))
            return Response<Guid>.ErrorResponse(ErrorMessages.NoAccessToAccount);

        var command = new CreateExpenseCommand
        {
            AccountId = dto.AccountId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Description = dto.Description,
            CategoryId = dto.CategoryId
        };

        return await _mediator.Send(command);
    }

    public async Task<Response<Guid>> DepositFundsAsync(Guid userId, DepositFundsDto dto)
    {
        var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
        if (!accounts.Data.Any(a => a.AccountId == dto.AccountId))
            return Response<Guid>.ErrorResponse(ErrorMessages.NoAccessToAccount);

        var command = new DepositFundsCommand
        {
            AccountId = dto.AccountId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Description = dto.Description
        };

        return await _mediator.Send(command);
    }

    public async Task<Response<Guid>> TransferFundsAsync(Guid userId, TransferFundsDto dto)
    {
        var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
        if (!accounts.Data.Any(a => a.AccountId == dto.SenderAccountId))
            return Response<Guid>.ErrorResponse(ErrorMessages.NoAccessToSenderAccount);

        var command = new TransferFundsCommand
        {
            SenderAccountId = dto.SenderAccountId,
            ReceiverAccountId = dto.ReceiverAccountId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Description = dto.Description
        };

        return await _mediator.Send(command);
    }
}
