namespace FynosAPI.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Gender { get; set; }
        public string? Category { get; set; }

        public bool InStock { get; set; }

        public string? ProductImage { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}