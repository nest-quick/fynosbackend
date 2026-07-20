using FynosAPI.Data;
using FynosAPI.Dtos.Categories;
using FynosAPI.Interfaces;
using FynosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FynosAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ApplicationDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(category => category.Name)
                .Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    ProductCount = category.Products.Count,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Where(category => category.Id == id)
                .Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    ProductCount = category.Products.Count,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (category is null)
            {
                _logger.LogWarning("Category {CategoryId} was not found", id);
            }

            return category;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var normalizedName = dto.Name.Trim();

            var nameExists = await _context.Categories
                .AnyAsync(category => category.Name.ToLower() == normalizedName.ToLower());

            if (nameExists)
            {
                throw new InvalidOperationException($"A category named '{normalizedName}' already exists.");
            }

            var category = new Category
            {
                Name = normalizedName,
                Description = dto.Description?.Trim(),
                ImageUrl = dto.ImageUrl?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Created category {CategoryId} with name {CategoryName}",
                category.Id,
                category.Name);

            return MapToDto(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category is null)
            {
                _logger.LogWarning("Cannot update category {CategoryId} because it was not found", id);

                return false;
            }

            var normalizedName = dto.Name.Trim();

            var duplicateExists = await _context.Categories
                .AnyAsync(existing =>
                    existing.Id != id &&
                    existing.Name.ToLower() == normalizedName.ToLower());

            if (duplicateExists)
            {
                throw new InvalidOperationException($"A category named '{normalizedName}' already exists.");
            }

            category.Name = normalizedName;
            category.Description = dto.Description?.Trim();
            category.ImageUrl = dto.ImageUrl?.Trim();
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated category {CategoryId}", id);

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(category => category.Products)
                .FirstOrDefaultAsync(category => category.Id == id);

            if (category is null)
            {
                _logger.LogWarning("Cannot delete category {CategoryId} because it was not found", id);

                return false;
            }

            if (category.Products.Count > 0)
            {
                throw new InvalidOperationException("A category containing products cannot be deleted.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Deleted category {CategoryId} with name {CategoryName}",
                category.Id,
                category.Name);

            return true;
        }

        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                ProductCount = category.Products.Count,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

    }
}
