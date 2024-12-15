using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using AhlApp.Shared.Security;
using MediatR;
using AhlApp.Domain.Constants; 

namespace AhlApp.Application.Features.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityHasher _securityHasher;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, ISecurityHasher securityHasher)
        {
            _unitOfWork = unitOfWork;
            _securityHasher = securityHasher;
        }

        public async Task<Response<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Response<Guid>.ErrorResponse(ErrorMessages.EmailAlreadyExists);
            }

            var (hash, salt) = _securityHasher.Hash(request.Password);
            var user = new User(request.Name, request.Email, hash, salt);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return Response<Guid>.SuccessResponse(user.Id);
        }
    }
}
