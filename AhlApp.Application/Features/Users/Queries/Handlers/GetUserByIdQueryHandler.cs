using AhlApp.Application.DTOs;
using AhlApp.Infrastructure.Repositories.Abstract;
using MediatR;
using AhlApp.Domain.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AhlApp.Application.Features.Users.Queries.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserWithAccountsAsync(request.UserId);

            if (user == null)
                throw new KeyNotFoundException(ErrorMessages.UserNotFound); 

            return new UserResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Accounts = user.Accounts.Select(a => new AccountResponseDto
                {
                    AccountId = a.Id,
                    UserId = a.UserId,
                    Balance = a.Balance.Amount,
                    Currency = a.Balance.Currency,
                    AccountName = a.AccountName
                }).ToList()
            };
        }
    }
}
