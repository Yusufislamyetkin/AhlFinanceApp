using AhlApp.Application.DTOs;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using MediatR;
using AhlApp.Domain.Constants;

namespace AhlApp.Application.Features.Accounts.Queries
{
    public class GetAccountsByUserQueryHandler : IRequestHandler<GetAccountsByUserQuery, Response<List<AccountResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountsByUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<AccountResponseDto>>> Handle(GetAccountsByUserQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.Accounts.GetAccountsByUserIdAsync(request.UserId);
            if (accounts == null || !accounts.Any())
            {
                return Response<List<AccountResponseDto>>.ErrorResponse(ErrorMessages.NoAccountsFoundForUser);
            }

            var accountDtos = accounts.Select(account => new AccountResponseDto
            {
                AccountId = account.Id,
                UserId = account.UserId,
                Balance = account.Balance.Amount,
                Currency = account.Balance.Currency,
                AccountName = account.AccountName
            }).ToList();

            return Response<List<AccountResponseDto>>.SuccessResponse(accountDtos);
        }
    }
}
