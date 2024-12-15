using AhlApp.Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AhlApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery { CategoryId = categoryId });
            if (!result.Success)
                return NotFound(new { Message = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpGet("{categoryId}/exists")]
        public async Task<IActionResult> IsValidCategory(Guid categoryId)
        {
            var result = await _mediator.Send(new IsValidCategoryQuery { CategoryId = categoryId });
            if (!result.Success)
                return BadRequest(new { Message = result.ErrorMessage });

            return Ok(new { Exists = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            if (!result.Success)
                return BadRequest(new { Message = result.ErrorMessage });

            return Ok(result.Data);
        }
    }
}
