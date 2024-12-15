using MediatR;
using AhlApp.Shared.Models;

namespace AhlApp.Application.Features.Categories.Queries
{
    public class IsValidCategoryQuery : IRequest<Response<bool>>
    {
        public Guid CategoryId { get; set; }
    }
}
