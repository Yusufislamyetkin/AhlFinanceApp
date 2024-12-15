using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Data;
using AhlApp.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AhlApp.Infrastructure.Repositories.Concrete
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetByIdAsync(Guid transactionId)
        {
            return await _context.Transactions
                .Include(t => t.Category) 
                .FirstOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Transactions
                .Where(t => t.SenderAccountId == accountId || t.ReceiverAccountId == accountId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task<List<Transaction>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Set<Transaction>()
                .Include(t => t.Category)
                .Where(t => t.Account.UserId == userId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByUserAsync(Guid userId)
        {
            return await _context.Set<Transaction>()
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Where(t =>
                    (t.Account != null && t.Account.UserId == userId) || // Kullanıcıya ait hesaplardan yapılan işlemler
                    (t.SenderAccount != null && t.SenderAccount.UserId == userId) || // Gönderen kullanıcı
                    (t.ReceiverAccount != null && t.ReceiverAccount.UserId == userId)) // Alıcı kullanıcı
                .ToListAsync();
        }

    }
}
