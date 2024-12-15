using MediatR;
using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using AhlApp.Domain.ValueObjects;
using AhlApp.Domain.Constants;

namespace AhlApp.Application.Features.Transactions.Commands.Handlers
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateExpenseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Guid>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var moneyResponse = Money.Create(request.Amount, request.Currency);
            if (!moneyResponse.Success)
                return Response<Guid>.ErrorResponse(moneyResponse.ErrorMessage);

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                return Response<Guid>.ErrorResponse(ErrorMessages.AccountNotFound);

            if (account.Balance.Amount < moneyResponse.Data.Amount)
                return Response<Guid>.ErrorResponse(ErrorMessages.InsufficientFunds);

            if (account.Balance.Currency != moneyResponse.Data.Currency)
                return Response<Guid>.ErrorResponse(ErrorMessages.DifferentCurrenciesNotAllowed);

            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (!category.Success)
                return Response<Guid>.ErrorResponse(ErrorMessages.InvalidCategory);

            var transaction = new Transaction(
                transactionType: TransactionType.Expense,
                amount: moneyResponse.Data,
                description: request.Description,
                categoryId: request.CategoryId,
                accountId: request.AccountId
            );

            var updatedBalance = Money.Subtract(account.Balance, moneyResponse.Data);
            account.UpdateBalance(updatedBalance.Data!);

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CommitAsync();

            return Response<Guid>.SuccessResponse(transaction.Id);
        }
    }
}
