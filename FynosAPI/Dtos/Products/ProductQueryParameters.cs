using System.ComponentModel.DataAnnotations;

namespace FynosAPI.Dtos.Products
{
    public class ProductQueryParameters
    {
        public string? Gender { get; set; }

        [Range(1, int.MaxValue)]
        public int? CategoryId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxPrice { get; set; }
        public bool? InStock { get; set; }
    }
}
