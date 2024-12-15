using AhlApp.Domain.Entities.Concrete;
using AhlApp.Domain.ValueObjects;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using MediatR;
using AhlApp.Domain.Constants; 

namespace AhlApp.Application.Features.Accounts.Commands
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı kontrolü
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
                return Response<Guid>.ErrorResponse(ErrorMessages.UserNotFound); 

            var account = new Account(request.UserId, new Money(request.InitialBalance, request.Currency), request.AccountNumber);

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.CommitAsync();

            return Response<Guid>.SuccessResponse(account.Id);
        }
    }
}
