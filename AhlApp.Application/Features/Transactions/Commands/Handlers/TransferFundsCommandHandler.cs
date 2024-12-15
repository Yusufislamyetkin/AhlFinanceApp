using MediatR;
using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using AhlApp.Domain.ValueObjects;
using AhlApp.Domain.Constants;

namespace AhlApp.Application.Features.Transactions.Commands.Handlers
{
    public class TransferFundsCommandHandler : IRequestHandler<TransferFundsCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferFundsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Guid>> Handle(TransferFundsCommand request, CancellationToken cancellationToken)
        {
            var moneyResponse = Money.Create(request.Amount, request.Currency);
            if (!moneyResponse.Success)
                return Response<Guid>.ErrorResponse(moneyResponse.ErrorMessage);

            var senderAccount = await _unitOfWork.Accounts.GetByIdAsync(request.SenderAccountId);
            if (senderAccount == null)
                return Response<Guid>.ErrorResponse(ErrorMessages.SenderAccountNotFound);

            var receiverAccount = await _unitOfWork.Accounts.GetByIdAsync(request.ReceiverAccountId);
            if (receiverAccount == null)
                return Response<Guid>.ErrorResponse(ErrorMessages.ReceiverAccountNotFound);

            if (senderAccount.Balance.Amount < moneyResponse.Data.Amount)
                return Response<Guid>.ErrorResponse(ErrorMessages.InsufficientBalanceInSenderAccount);

            var transferCategory = await _unitOfWork.Categories.GetByNameAsync("Transfer");
            if (transferCategory == null)
                return Response<Guid>.ErrorResponse(ErrorMessages.TransferCategoryNotFound);

            var transaction = new Transaction(
                transactionType: TransactionType.Transfer,
                amount: moneyResponse.Data,
                description: request.Description,
                categoryId: transferCategory.Id,
                senderAccountId: request.SenderAccountId,
                receiverAccountId: request.ReceiverAccountId
            );

            var senderUpdatedBalance = Money.Subtract(senderAccount.Balance, moneyResponse.Data);
            if (!senderUpdatedBalance.Success)
                return Response<Guid>.ErrorResponse(senderUpdatedBalance.ErrorMessage);
            senderAccount.UpdateBalance(senderUpdatedBalance.Data!);

            var receiverUpdatedBalance = Money.Add(receiverAccount.Balance, moneyResponse.Data);
            if (!receiverUpdatedBalance.Success)
                return Response<Guid>.ErrorResponse(receiverUpdatedBalance.ErrorMessage);
            receiverAccount.UpdateBalance(receiverUpdatedBalance.Data!);

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CommitAsync();

            return Response<Guid>.SuccessResponse(transaction.Id);
        }
    }
}
