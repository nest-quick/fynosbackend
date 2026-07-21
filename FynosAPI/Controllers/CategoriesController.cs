using Microsoft.AspNetCore.Mvc;
using FynosAPI.Dtos.Categories;
using FynosAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FynosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category is null)
            {
                return NotFound(new
                {
                    message = $"Category {id} was not found."
                });
            }

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(
            [FromBody] CreateCategoryDto dto)
        {
            var category = await _categoryService.CreateCategoryAsync(dto);

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.Id },
                category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] UpdateCategoryDto dto)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = $"Category {id} was not found."
                });
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = $"Category {id} was not found."
                });
            }

            return NoContent();
        }
    }
}
