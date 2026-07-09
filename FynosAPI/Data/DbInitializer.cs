using FynosAPI.Models;

namespace FynosAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Products.Any())
            {
                return;
            }

            var products = new List<Product>()
            {
                new Product
                {
                    Name = "FYNOS Muay Thai Shorts",
                    Description = "Lightweight training shorts built for striking, grappling, and everyday training.",
                    Price = 64.99m,
                    Gender = "Men",
                    Category = "Shorts",
                    StockQuantity = 20,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Fight Shorts",
                    Description = "Durable fight shorts with a clean athletic fit.",
                    Price = 69.99m,
                    Gender = "Men",
                    Category = "Shorts",
                    StockQuantity = 15,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Performance Rashguard",
                    Description = "Compression rashguard designed for no-gi training and high-intensity sessions.",
                    Price = 54.99m,
                    Gender = "Men",
                    Category = "Rashguards",
                    StockQuantity = 18,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Oversized Tee",
                    Description = "Oversized premium tee with a clean streetwear silhouette.",
                    Price = 39.99m,
                    Gender = "Unisex",
                    Category = "T-Shirts",
                    StockQuantity = 30,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Heavyweight Hoodie",
                    Description = "Heavyweight hoodie designed for comfort, training, and daily wear.",
                    Price = 79.99m,
                    Gender = "Unisex",
                    Category = "Hoodies",
                    StockQuantity = 12,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Women's Training Shorts",
                    Description = "Training shorts designed for movement, comfort, and performance.",
                    Price = 59.99m,
                    Gender = "Women",
                    Category = "Shorts",
                    StockQuantity = 16,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Women's Crop Tee",
                    Description = "Cropped training tee with a clean athletic look.",
                    Price = 34.99m,
                    Gender = "Women",
                    Category = "T-Shirts",
                    StockQuantity = 22,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Backpack",
                    Description = "Everyday training backpack for gym gear and essentials.",
                    Price = 49.99m,
                    Gender = "Unisex",
                    Category = "Accessories",
                    StockQuantity = 10,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
