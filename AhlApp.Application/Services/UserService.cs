using AhlApp.Application.DTOs;
using AhlApp.Application.Interfaces;
using AhlApp.Application.Features.Users.Commands;
using AhlApp.Application.Features.Users.Queries;
using MediatR;
using AhlApp.Application.DTOs.UserDTOs;
using AhlApp.Shared.Models;
using AhlApp.Domain.Constants;

namespace AhlApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<UserResponseDto>> RegisterUserAsync(string name, string email, string password)
        {
            var command = new RegisterUserCommand
            {
                Name = name,
                Email = email,
                Password = password
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Response<UserResponseDto>.ErrorResponse(result.ErrorMessage);
            }

            // Komut başarılıysa kullanıcı detaylarını getir
            var userDetails = await GetUserByIdAsync(result.Data);
            if (userDetails == null)
            {
                return Response<UserResponseDto>.ErrorResponse(ErrorMessages.FailedToRetrieveUserDetails);
            }

            return Response<UserResponseDto>.SuccessResponse(userDetails);
        }


        public async Task<UserResponseDto> GetUserByIdAsync(Guid userId)
        {
            var query = new GetUserByIdQuery { UserId = userId };
            return await _mediator.Send(query);
        }

    
    }
}
