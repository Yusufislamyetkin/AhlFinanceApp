using AhlApp.Infrastructure.Extensions;
using AhlApp.Application.Extensions;
using AhlApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using AhlApp.Infrastructure.Security;
using System.Reflection;
using MediatR;

var builder = WebApplication.CreateBuilder(args);



Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


var configuration = builder.Configuration;



builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("RedisConnection");
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddInfrastructureServices();


builder.Services.AddApplicationServices();


var serviceProvider = builder.Services.BuildServiceProvider();
var mediator = serviceProvider.GetService<IMediator>();
if (mediator == null)
{
    throw new Exception("MediatR is not properly registered.");
}


var jwtSettings = builder.Configuration.GetSection("Jwt");


builder.Services.Configure<JwtSettings>(jwtSettings);


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
        };
    });


builder.Services.AddAuthorization();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AhlApp API v1");
        options.RoutePrefix = string.Empty; 
    });
}
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// **Uygulamayý Çalýþtýr**
app.Run();
