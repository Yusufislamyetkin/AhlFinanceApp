using AhlApp.Domain.Entities.Concrete;

namespace AhlApp.Infrastructure.Repositories.Abstract
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByNumberAsync(string accountName);
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(Guid userId);
    }
}
