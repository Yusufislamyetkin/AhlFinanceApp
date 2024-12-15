using Microsoft.AspNetCore.Http;
using AhlApp.Domain.Constants;
using System;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        // Middleware kullanıcı kimliğini doğruladığından, burada null kontrolü yalnızca hata fırlatmak için yapılır.
        return Guid.Parse(context.Items["UserId"]?.ToString()
            ?? throw new UnauthorizedAccessException(ErrorMessages.TokenMissing));
    }
}
