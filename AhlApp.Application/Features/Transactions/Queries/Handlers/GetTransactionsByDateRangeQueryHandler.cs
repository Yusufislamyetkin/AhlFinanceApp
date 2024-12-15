using AhlApp.Application.DTOs;
using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Infrastructure.Repositories.Abstract;
using MediatR;

namespace AhlApp.Application.Features.Transactions.Queries.Handlers
{
    public class GetTransactionsByDateRangeQueryHandler : IRequestHandler<GetTransactionsByDateRangeQuery, IEnumerable<TransactionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionsByDateRangeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                return Enumerable.Empty<TransactionDto>();

            var transactions = await _unitOfWork.Transactions.GetByDateRangeAsync(request.StartDate, request.EndDate);

            var accountTransactions = transactions
                .Where(t => t.SenderAccountId == request.AccountId || t.ReceiverAccountId == request.AccountId)
                .Select(t => new TransactionDto
                {
                    TransactionId = t.Id,
                    SenderAccountId = t.SenderAccountId,
                    ReceiverAccountId = t.ReceiverAccountId,
                    Amount = t.Amount.Amount,
                    Currency = t.Amount.Currency,
                    Description = t.Description,
                    CategoryId = t.CategoryId,
                    TransactionDate = t.TransactionDate
                }).ToList();

            return accountTransactions;
        }
    }
}
