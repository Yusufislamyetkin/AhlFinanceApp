using AhlApp.Application.DTOs;
using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Infrastructure.Repositories.Abstract;
using MediatR;

namespace AhlApp.Application.Features.Transactions.Queries.Handlers
{
    public class GetTransactionsByAccountQueryHandler : IRequestHandler<GetTransactionsByAccountQuery, IEnumerable<TransactionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionsByAccountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsByAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                return Enumerable.Empty<TransactionDto>();

            var transactions = await _unitOfWork.Transactions.GetByAccountIdAsync(request.AccountId);

            var accountTransactions = transactions.Select(t => new TransactionDto
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
