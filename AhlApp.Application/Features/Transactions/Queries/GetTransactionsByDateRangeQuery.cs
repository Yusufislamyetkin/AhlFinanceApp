using AhlApp.Application.DTOs;
using AhlApp.Application.DTOs.TransactionDTOs;
using MediatR;

namespace AhlApp.Application.Features.Transactions.Queries
{
    public class GetTransactionsByDateRangeQuery : IRequest<IEnumerable<TransactionDto>>
    {
        public Guid AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
