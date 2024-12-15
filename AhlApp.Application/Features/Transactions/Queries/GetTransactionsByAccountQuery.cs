using AhlApp.Application.DTOs;
using AhlApp.Application.DTOs.TransactionDTOs;
using MediatR;

namespace AhlApp.Application.Features.Transactions.Queries
{
    public class GetTransactionsByAccountQuery : IRequest<IEnumerable<TransactionDto>>
    {
        public Guid AccountId { get; set; }
    }
}
