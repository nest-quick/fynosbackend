using Microsoft.EntityFrameworkCore;
using FynosAPI.Data;
using FynosAPI.Dtos;
using FynosAPI.Interfaces;
using FynosAPI.Models;
using FynosAPI.Dtos.Products;

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

        public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(ProductQueryParameters queryParameters)
        {
            if (queryParameters.MinPrice.HasValue &&
                queryParameters.MaxPrice.HasValue &&
                queryParameters.MinPrice > queryParameters.MaxPrice)
            {
                throw new ArgumentException(
                    "Minimum price cannot be greater than maximum price.");
            }

            _logger.LogInformation(
                    "Retrieving products with Gender: {Gender}, CategoryId: {CategoryId}, MinPrice: {MinPrice}, MaxPrice: {MaxPrice}, InStock: {InStock}",
                    queryParameters.Gender,
                    queryParameters.CategoryId,
                    queryParameters.MinPrice,
                    queryParameters.MaxPrice,
                    queryParameters.InStock);

            var query = _context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.Gender))
            {
                var gender = queryParameters.Gender.Trim();

                query = query.Where(product =>
                    product.Gender != null &&
                    product.Gender.ToLower() == gender.ToLower());
            }

            if (queryParameters.CategoryId.HasValue)
            {
                query = query.Where(product =>
                    product.CategoryId == queryParameters.CategoryId.Value);
            }

            if (queryParameters.MinPrice.HasValue)
            {
                query = query.Where(product =>
                    product.Price >= queryParameters.MinPrice.Value);
            }

            if (queryParameters.MaxPrice.HasValue)
            {
                query = query.Where(product =>
                    product.Price <= queryParameters.MaxPrice.Value);
            }

            if (queryParameters.InStock.HasValue)
            {
                if (queryParameters.InStock.Value)
                {
                    query = query.Where(product =>
                        product.StockQuantity > 0);
                }
                else
                {
                    query = query.Where(product =>
                        product.StockQuantity == 0);
                }
            }

            var products = await query
                .OrderBy(product => product.Name)
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Gender = product.Gender,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    InStock = product.StockQuantity > 0,
                    ProductImage = product.ProductImage,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt
                })
                .ToListAsync();

            _logger.LogInformation(
                "Retrieved {ProductCount} products",
                 products.Count);

            return products;
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