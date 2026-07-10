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

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(string? gender)
        {
            var query = _context.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(p => p.Gender != null && p.Gender.ToLower() == gender.ToLower());
            }

            var products = await query.ToListAsync();

            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            return MapToDto(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Gender = dto.Gender,
                Category = dto.Category,
                StockQuantity = dto.StockQuantity,
                ProductImage = dto.ProductImage,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Gender = dto.Gender;
            product.Category = dto.Category;
            product.StockQuantity = dto.StockQuantity;
            product.ProductImage = dto.ProductImage;

            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

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
                Category = product.Category,
                InStock = product.StockQuantity > 0,
                ProductImage = product.ProductImage,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }


    }
}