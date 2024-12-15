using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Data;
using AhlApp.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AhlApp.Infrastructure.Repositories.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserWithAccountsAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Accounts)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }


    }
}
