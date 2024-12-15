using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AhlApp.Domain.Constants; 

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] _whitelistedRoutes = new[]
    {
        "/api/auth/login",    
        "/api/auth/register"  
    };

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Whitelist kontrolü
        if (_whitelistedRoutes.Any(route => context.Request.Path.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        // Authorization başlığından token'ı al
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            await HandleUnauthorizedResponse(context, ErrorMessages.TokenMissing);
            return;
        }

        try
        {
            // Token doğrulama işlemi
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                await HandleUnauthorizedResponse(context, ErrorMessages.InvalidToken);
                return;
            }

            // Kullanıcı kimliğini HttpContext'e ekle
            context.Items["UserId"] = userIdClaim.Value;
        }
        catch
        {
            await HandleUnauthorizedResponse(context, ErrorMessages.TokenExpiredOrInvalid);
            return;
        }

        await _next(context);
    }

    private async Task HandleUnauthorizedResponse(HttpContext context, string errorMessage)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsJsonAsync(new { Message = errorMessage });
    }
}
