using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    public class SportifyDbContext : DbContext
    {
        private readonly ILogger<SportifyDbContext> _logger;

        public SportifyDbContext(DbContextOptions<SportifyDbContext> options, ILogger<SportifyDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                // Specify precision for decimal properties
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                // Configure relationships
                entity.HasOne(p => p.ProductBrand)
                      .WithMany()
                      .HasForeignKey(p => p.ProductBrandId);

                entity.HasOne(p => p.ProductType)
                      .WithMany()
                      .HasForeignKey(p => p.ProductTypeId);

                entity.HasMany(p => p.Reviews)
                      .WithOne(r => r.Product)
                      .HasForeignKey(r => r.ProductId);
            });

            // Configure Review entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasOne(r => r.Product)
                      .WithMany(p => p.Reviews)
                      .HasForeignKey(r => r.ProductId);
            });

            // Load and seed data
            var productBrands = LoadSeedData<ProductBrand>("brands.json", _logger);
            var productTypes = LoadSeedData<ProductType>("types.json", _logger);
            var products = LoadSeedData<Product>("products.json", _logger);
            var reviews = LoadSeedData<Review>("reviews.json", _logger);

            // Assign negative IDs for seed data (start from negative numbers)
            int brandId = -1;
            foreach (var brand in productBrands)
            {
                brand.Id = brandId--;
            }

            int typeId = -1;
            foreach (var type in productTypes)
            {
                type.Id = typeId--;
            }

            int productId = -1;
            foreach (var product in products)
            {
                product.Id = productId--;
            }

            int reviewId = -1;
            foreach (var review in reviews)
            {
                review.Id = reviewId--;
            }

            // Seed data
            modelBuilder.Entity<ProductBrand>().HasData(productBrands);
            modelBuilder.Entity<ProductType>().HasData(productTypes);
            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<Review>().HasData(reviews);
        }

        private List<T> LoadSeedData<T>(string fileName, ILogger logger) where T : class
        {
            var basePath = AppContext.BaseDirectory;
            var fullPath = Path.Combine(basePath, "SeedData", fileName);

            logger.LogDebug($"Base path: {basePath}");
            logger.LogDebug($"Full path: {fullPath}");

            if (!File.Exists(fullPath))
            {
                logger.LogError($"File not found at path: {fullPath}");
                throw new FileNotFoundException($"Could not find the JSON file at path: {fullPath}");
            }

            var jsonData = File.ReadAllText(fullPath);
            if (string.IsNullOrEmpty(jsonData))
            {
                logger.LogError($"The JSON file {fileName} is empty.");
                throw new InvalidOperationException($"The JSON file {fileName} is empty.");
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var result = JsonSerializer.Deserialize<List<T>>(jsonData, options);
                if (result == null)
                {
                    logger.LogError($"Failed to deserialize JSON data from {fileName}");
                    throw new InvalidOperationException($"Failed to deserialize JSON data from {fileName}");
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error during deserialization of {fileName}");
                throw;
            }
        }
    }
}