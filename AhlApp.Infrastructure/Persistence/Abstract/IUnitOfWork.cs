using System.Threading.Tasks;
using AhlApp.Infrastructure.Repositories.Abstract;

namespace AhlApp.Infrastructure.Repositories.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        ITransactionRepository Transactions { get; }
        ICategoryRepository Categories { get; } 

        Task<int> CommitAsync();
    }
}
