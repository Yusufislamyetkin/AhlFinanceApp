using AhlApp.Domain.Entities.Concrete;

namespace AhlApp.Infrastructure.Repositories.Abstract
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetByIdAsync(Guid transactionId);      
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddAsync(Transaction transaction);            

        Task<List<Transaction>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetByUserAsync(Guid userId);
    }
}
