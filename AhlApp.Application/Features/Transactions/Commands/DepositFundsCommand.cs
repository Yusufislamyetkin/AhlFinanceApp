using AhlApp.Shared.Models;
using MediatR;

namespace AhlApp.Application.Features.Transactions.Commands
{
    public class DepositFundsCommand : IRequest<Response<Guid>>
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
    }
}
