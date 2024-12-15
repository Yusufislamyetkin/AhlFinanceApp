using MediatR;
using AhlApp.Application.Features.Categories.Queries;
using AhlApp.Application.Interfaces;
using AhlApp.Domain.Entities.Concrete;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Features.Categories.Queries.Handlers
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Response<Category>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryByIdQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Response<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategoryByIdAsync(request.CategoryId);
        }
    }
}
