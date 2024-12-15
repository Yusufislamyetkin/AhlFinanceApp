using AhlApp.Shared.Models;
using MediatR;

namespace AhlApp.Application.Features.Users.Commands
{
    public class RegisterUserCommand : IRequest<Response<Guid>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
