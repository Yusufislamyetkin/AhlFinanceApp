using AhlApp.Domain.Entities.Concrete;
using AhlApp.Shared.Models;

namespace AhlApp.Infrastructure.Repositories.Abstract
{
    public interface ICategoryRepository
    {
        Task<Response<Category>> GetByIdAsync(Guid categoryId);
        Task<Response<bool>> ExistsAsync(Guid categoryId);
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByNameAsync(string name);
    }
}
