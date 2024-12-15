using AhlApp.Application.DTOs;
using MediatR;

namespace AhlApp.Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserResponseDto>
    {
        public Guid UserId { get; set; }
    }
}
