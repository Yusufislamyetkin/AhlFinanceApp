using AhlApp.Shared.Models;
using MediatR;

namespace AhlApp.Application.Features.Accounts.Commands
{
    public class CreateAccountCommand : IRequest<Response<Guid>>
    {
        public Guid UserId { get; set; }
        public decimal InitialBalance { get; set; }
        public string Currency { get; set; }
        public string AccountNumber { get; set; }
    }
}
