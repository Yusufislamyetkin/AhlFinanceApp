using AhlApp.Application.DTOs;
using AhlApp.Application.Interfaces;
using AhlApp.Application.Features.Accounts.Commands;
using AhlApp.Application.Features.Accounts.Queries;
using AhlApp.Shared.Models;
using MediatR;

namespace AhlApp.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMediator _mediator;

        public AccountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<List<AccountResponseDto>>> GetAccountsByUserIdAsync(Guid userId)
        {
            var query = new GetAccountsByUserQuery { UserId = userId };
            return await _mediator.Send(query);
        }
    }
}
