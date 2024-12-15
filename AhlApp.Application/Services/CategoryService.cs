using AhlApp.Application.Features.Categories.Queries;
using AhlApp.Application.Interfaces;
using AhlApp.Domain.Entities.Concrete;
using AhlApp.Infrastructure.Repositories.Abstract;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<Category>> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _categoryRepository.GetByIdAsync(categoryId);
        }

        public async Task<Response<bool>> IsValidCategoryAsync(Guid categoryId)
        {
            return await _categoryRepository.ExistsAsync(categoryId);
        }
    
    }
}
