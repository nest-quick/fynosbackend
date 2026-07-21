using FynosAPI.Dtos;
using FynosAPI.Models;
using FynosAPI.Dtos.Products;

namespace FynosAPI.Interfaces
{
    public interface IProductService
    {
        //Task<IEnumerable<ProductDto>> GetProductsAsync(string? gender);
        Task<IReadOnlyList<ProductDto>> GetProductsAsync(ProductQueryParameters queryParameters);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<bool> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int id);
    }
}