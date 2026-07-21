using FynosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FynosAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Categories.AnyAsync() || await context.Products.AnyAsync())
            {
                return;
            }

            var muayThaiShorts = new Category
            {
                Name = "Muay Thai Shorts",
                Description = "Traditional and modern Muay Thai shorts.",
                CreatedAt = DateTime.UtcNow
            };

            var fightShorts = new Category
            {
                Name = "Fight Shorts",
                Description = "Performance shorts for MMA and grappling.",
                CreatedAt = DateTime.UtcNow
            };

            var rashguards = new Category
            {
                Name = "Rashguards",
                Description = "Compression tops for grappling and training.",
                CreatedAt = DateTime.UtcNow
            };

            var tshirts = new Category
            {
                Name = "T-Shirts",
                Description = "FYNOS lifestyle and training shirts.",
                CreatedAt = DateTime.UtcNow
            };

            var hoodies = new Category
            {
                Name = "Hoodies",
                Description = "Heavyweight FYNOS hoodies.",
                CreatedAt = DateTime.UtcNow
            };

            var accessories = new Category
            {
                Name = "Accessories",
                Description = "Bags, hats, and training accessories.",
                CreatedAt = DateTime.UtcNow
            };

            context.Categories.AddRange(
                muayThaiShorts,
                fightShorts,
                rashguards,
                tshirts,
                hoodies,
                accessories
            );

            await context.SaveChangesAsync();

            var products = new List<Product>()
            {
                new Product
                {
                    Name = "FYNOS Muay Thai Shorts",
                    Description = "Lightweight training shorts built for striking, grappling, and everyday training.",
                    Price = 64.99m,
                    Gender = "Men",
                    CategoryId = muayThaiShorts.Id,
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
                    CategoryId = fightShorts.Id,
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
                    CategoryId = rashguards.Id,
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
                    CategoryId = tshirts.Id,
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
                    CategoryId = hoodies.Id,
                    StockQuantity = 12,
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Women's Fight Shorts",
                    Description = "Fighting shorts designed for movement, comfort, and performance.",
                    Price = 59.99m,
                    Gender = "Women",
                    CategoryId = fightShorts.Id,
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
                    CategoryId = rashguards.Id,
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
                    CategoryId = accessories.Id,
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
