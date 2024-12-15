using MediatR;
using AhlApp.Domain.Entities.Concrete;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Features.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<Response<Category>>
    {
        public Guid CategoryId { get; set; }
    }
}
