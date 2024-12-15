using AhlApp.Domain.Entities.Concrete;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Response<Category>> GetCategoryByIdAsync(Guid categoryId);
        Task<Response<bool>> IsValidCategoryAsync(Guid categoryId);
    }
}
