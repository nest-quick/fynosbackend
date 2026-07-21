using System.ComponentModel.DataAnnotations;

namespace FynosAPI.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Gender { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        public int StockQuantity { get; set; }
        public string? ProductImage { get; set; }
    }
}