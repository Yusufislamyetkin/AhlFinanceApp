using AhlApp.Domain.Entities.Concrete;

namespace AhlApp.Infrastructure.Repositories.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetUserWithAccountsAsync(Guid userId);

    }
}
