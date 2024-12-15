using AhlApp.Infrastructure.Caching;
using AhlApp.Infrastructure.Repositories.Concrete;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Caching;
using AhlApp.Shared.Logging;
using Microsoft.Extensions.DependencyInjection;
using AhlApp.Infrastructure.Logging.Concrete;
using AhlApp.Infrastructure.Security;
using AhlApp.Shared.Security;
using AhlApp.Infrastructure.Security.Abstract;
using AhlApp.Infrastructure.Repositories;
using AhlApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace AhlApp.Infrastructure.Extensions
{
    public static class InfraServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {


            // Caching ve Logging
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<ILoggerService, SerilogLoggerService>();
            services.AddScoped<ISecurityHasher, SecurityHasher>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();


            // Unit of Work ve Repository'ler
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
