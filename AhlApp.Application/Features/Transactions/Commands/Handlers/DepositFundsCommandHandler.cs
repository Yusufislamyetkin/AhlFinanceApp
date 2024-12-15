using MediatR;
using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using AhlApp.Domain.ValueObjects;
using AhlApp.Domain.Constants;

namespace AhlApp.Application.Features.Transactions.Commands.Handlers
{
    public class DepositFundsCommandHandler : IRequestHandler<DepositFundsCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepositFundsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Guid>> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
        {
            var moneyResponse = Money.Create(request.Amount, request.Currency);
            if (!moneyResponse.Success)
                return Response<Guid>.ErrorResponse(moneyResponse.ErrorMessage);

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                return Response<Guid>.ErrorResponse(ErrorMessages.AccountNotFound);

            var depositCategory = await _unitOfWork.Categories.GetByNameAsync("Deposit");
            if (depositCategory == null)
                return Response<Guid>.ErrorResponse(ErrorMessages.DepositCategoryNotFound);

            var updatedBalance = Money.Add(account.Balance, moneyResponse.Data);
            if (!updatedBalance.Success)
                return Response<Guid>.ErrorResponse(updatedBalance.ErrorMessage);

            account.UpdateBalance(updatedBalance.Data!);

            var transaction = new Transaction(
                transactionType: TransactionType.Deposit,
                amount: moneyResponse.Data,
                description: request.Description,
                categoryId: depositCategory.Id,
                accountId: request.AccountId
            );

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CommitAsync();

            return Response<Guid>.SuccessResponse(transaction.Id);
        }
    }
}
