using AhlApp.Application.DTOs;
using AhlApp.Shared.Models;
using MediatR;

namespace AhlApp.Application.Features.Accounts.Queries
{
    public class GetAccountDetailsQuery : IRequest<Response<AccountResponseDto>>
    {
        public Guid AccountId { get; set; }
    }
}
