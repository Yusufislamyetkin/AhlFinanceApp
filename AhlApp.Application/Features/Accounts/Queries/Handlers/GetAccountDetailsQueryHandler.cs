using AhlApp.Application.DTOs;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using MediatR;
using AhlApp.Domain.Constants; 

namespace AhlApp.Application.Features.Accounts.Queries
{
    public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQuery, Response<AccountResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<AccountResponseDto>> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                return Response<AccountResponseDto>.ErrorResponse(ErrorMessages.AccountNotFound); 

            var dto = new AccountResponseDto
            {
                AccountId = account.Id,
                UserId = account.UserId,
                Balance = account.Balance.Amount,
                Currency = account.Balance.Currency,
                AccountName = account.AccountName
            };

            return Response<AccountResponseDto>.SuccessResponse(dto);
        }
    }
}
