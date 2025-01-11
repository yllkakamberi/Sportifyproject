using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace Infrastructure.Data;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Specify precision for decimal properties
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");  // Specify precision and scale

        // Load data from JSON files
        var productBrands = LoadSeedData<ProductBrand>("brands.json", _logger);
        var productTypes = LoadSeedData<ProductType>("types.json", _logger);
        var products = LoadSeedData<Product>("products.json", _logger);

        // Assign negative IDs for seed data
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

        modelBuilder.Entity<ProductBrand>().HasData(productBrands);
        modelBuilder.Entity<ProductType>().HasData(productTypes);
        modelBuilder.Entity<Product>().HasData(products);
    }

    private List<T> LoadSeedData<T>(string fileName, ILogger logger) where T : class
    {
        var basePath = AppContext.BaseDirectory;
        var fullPath = Path.Combine(basePath, "SeedData", fileName);

        _logger.LogDebug($"Base path: {basePath}");
        _logger.LogDebug($"Full path: {fullPath}");

        _logger.LogDebug($"Looking for file at path: {fullPath}");
        if (!File.Exists(fullPath))
        {
            _logger.LogError($"File not found at path: {fullPath}");
            throw new FileNotFoundException($"Could not find the JSON file at path: {fullPath}");
        }

        // Only read and deserialize the file if it exists
        var jsonData = File.ReadAllText(fullPath);
        var result = JsonSerializer.Deserialize<List<T>>(jsonData);

        if (result == null)
        {
            logger.LogError($"Failed to deserialize JSON data from {fileName}");
            logger.LogDebug($"Content of {fileName}: {jsonData}");
            throw new InvalidOperationException($"Failed to deserialize JSON data from {fileName}");
        }

        return result;



    }

}
