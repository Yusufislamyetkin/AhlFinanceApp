using MediatR;
using AhlApp.Application.Features.Categories.Queries;
using AhlApp.Application.Interfaces;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Features.Categories.Queries.Handlers
{
    public class IsValidCategoryQueryHandler : IRequestHandler<IsValidCategoryQuery, Response<bool>>
    {
        private readonly ICategoryService _categoryService;

        public IsValidCategoryQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Response<bool>> Handle(IsValidCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.IsValidCategoryAsync(request.CategoryId);
        }
    }
}
