using Microsoft.EntityFrameworkCore;
using FynosAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FynosAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories {  get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasIndex(category => category.Name)
                .IsUnique();

            builder.Entity<Category>()
                .Property(category => category.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}