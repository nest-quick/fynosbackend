namespace FynosAPI.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public string? Gender { get; set; }
        public string? Category { get; set; }
        public int StockQuantity { get; set; }
        public string? ProductImage { get; set; }
    }
}