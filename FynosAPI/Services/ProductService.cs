using Microsoft.EntityFrameworkCore;
using FynosAPI.Data;
using FynosAPI.Dtos;
using FynosAPI.Interfaces;
using FynosAPI.Models;

namespace FynosAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(string? gender)
        {
            _logger.LogInformation(
                "Retrieving products. Gender filter: {Gender}",
                gender ?? "None");

            var query = _context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(p => p.Gender != null && p.Gender.ToLower() == gender.ToLower());
            }

            var products = await query.ToListAsync();
            
            _logger.LogInformation(
                "Retrieved {ProductCount} products",
                products.Count);

            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            _logger.LogInformation(
                "Retrieving product {ProductId}",
                id);

            var product = await _context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                _logger.LogWarning(
                    "Product {ProductId} was not found",
                    id);

                return null;
            }

            return MapToDto(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(category => category.Id == dto.CategoryId);

            if (!categoryExists)
            {
                throw new ArgumentException(
                    $"Category {dto.CategoryId} does not exist.");
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Gender = dto.Gender,
                CategoryId = dto.CategoryId,
                StockQuantity = dto.StockQuantity,
                ProductImage = dto.ProductImage,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Created product {ProductId} with name {ProductName}",
                product.Id,
                product.Name);

            // Reload the product with its Category navigation property.
            var createdProduct = await _context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .FirstAsync(product => product.Id == product.Id);

            return MapToDto(product);
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning(
                    "Cannot update product {ProductId} because it was not found",
                    id);

                return false;
            }

            var categoryExists = await _context.Categories
                .AnyAsync(category => category.Id == dto.CategoryId);

            if (!categoryExists)
            {
                throw new ArgumentException(
                    $"Category {dto.CategoryId} does not exist.");
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Gender = dto.Gender;
            product.CategoryId = dto.CategoryId;
            product.StockQuantity = dto.StockQuantity;
            product.ProductImage = dto.ProductImage;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Updated product {ProductId}",
                id);

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning(
                    "Cannot delete product {ProductId} because it was not found",
                    id);

                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Deleted product {ProductId} with name {ProductName}",
                product.Id,
                product.Name);

            return true;
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Gender = product.Gender,
                CategoryId = product.CategoryId,
                InStock = product.StockQuantity > 0,
                ProductImage = product.ProductImage,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }


    }
}