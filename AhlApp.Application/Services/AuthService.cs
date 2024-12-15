using AhlApp.Application.Interfaces;
using AhlApp.Domain.Constants;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Infrastructure.Security.Abstract;
using AhlApp.Shared.Models;
using AhlApp.Shared.Security; 

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityHasher _securityHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUserRepository userRepository, ISecurityHasher securityHasher, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _securityHasher = securityHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Response<string>> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null || !_securityHasher.Validate(password, user.PasswordHash, user.PasswordSalt))
        {
            return Response<string>.ErrorResponse(ErrorMessages.InvalidCredentials);
        }

        var token = _jwtTokenService.GenerateToken(user.Id.ToString(), "User");
        return Response<string>.SuccessResponse(token);
    }
}
