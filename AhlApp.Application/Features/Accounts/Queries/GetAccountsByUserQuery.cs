using AhlApp.Application.DTOs;
using AhlApp.Shared.Models;
using MediatR;

namespace AhlApp.Application.Features.Accounts.Queries
{
    public class GetAccountsByUserQuery : IRequest<Response<List<AccountResponseDto>>>
    {
        public Guid UserId { get; set; }
    }
}
