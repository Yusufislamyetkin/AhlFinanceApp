using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Data;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using AhlApp.Domain.Constants;

namespace AhlApp.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<Category>> GetByIdAsync(Guid categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category == null)
                return Response<Category>.ErrorResponse(ErrorMessages.CategoryNotFound);

            return Response<Category>.SuccessResponse(category);
        }

        public async Task<Response<bool>> ExistsAsync(Guid categoryId)
        {
            var exists = await _dbContext.Categories.AnyAsync(c => c.Id == categoryId);
            return Response<bool>.SuccessResponse(exists);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ErrorMessages.CategoryNameCannotBeEmpty, nameof(name));

            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
