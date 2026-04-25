using FynosAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;
using System.Xml.Linq;

namespace FynosAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if(context.Products.Any())
            {
                return;
            }

            var products = new List<Product>()
            {
                new Product
                {
                    Name = "FYNOS Essential Tee",
                    Description = "A premium everyday t-shirt with a clean streetwear look.",
                    Price = 29.99m,
                    Gender = "Men",
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Street Hoodie",
                    Description = "Heavyweight hoodie designed for comfort and style.",
                    Price = 64.99m,
                    Gender = "Men",
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Logo Cap",
                    Description = "Classic cap featuring the FYNOS logo.",
                    Price = 34.99m,
                    Gender = "Men",
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Crop Top",
                    Description = "Classic top featuring the FYNOS logo.",
                    Price = 34.99m,
                    Gender = "Women",
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "FYNOS Tights",
                    Description = "Classic Tights featuring the FYNOS logo.",
                    Price = 34.99m,
                    Gender = "Women",
                    ProductImage = "/images/muaythaishorts.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
