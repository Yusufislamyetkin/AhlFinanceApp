using AhlApp.Infrastructure.Data;
using AhlApp.Infrastructure.Repositories.Abstract;

namespace AhlApp.Infrastructure.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public IAccountRepository Accounts { get; }
        public ITransactionRepository Transactions { get; }
        public ICategoryRepository Categories { get; } 

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository users,
            IAccountRepository accounts,
            ITransactionRepository transactions,
            ICategoryRepository categories)
        {
            _context = context;
            Users = users;
            Accounts = accounts;
            Transactions = transactions;
            Categories = categories;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
