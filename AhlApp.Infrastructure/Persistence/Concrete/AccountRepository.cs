using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Data;
using AhlApp.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AhlApp.Infrastructure.Repositories.Concrete
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Account> GetAccountByNumberAsync(string accountName)
        {
            return await _context.Set<Account>()
                .FirstOrDefaultAsync(a => a.AccountName == accountName);
        }

        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(Guid userId)
        {
            // Kullanıcıya ait tüm hesapları getirir.
            return await _context.Set<Account>()
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
