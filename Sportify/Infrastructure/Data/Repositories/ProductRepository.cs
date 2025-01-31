using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly SportifyDbContext _context;

        public ProductRepository(SportifyDbContext context) : base(context)
        {
            _context = context;
        }

        // Implement the missing method
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews) // Include reviews for average rating calculations
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Other methods remain unchanged
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews) // Include reviews for average rating calculations
                .ToListAsync();
        }

        public async Task<IList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.AsNoTracking().ToListAsync();
        }

        public async Task<IList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName)
        {
            if (string.IsNullOrWhiteSpace(brandName))
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Where(p => p.ProductBrand.Name.ToLower().Contains(brandName.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByTypeAsync(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Where(p => p.ProductType.Name.ToLower().Contains(typeName.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            if (minPrice > maxPrice)
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .OrderBy(p => p.Price)  // Sorting in ascending order
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWithSufficientStockAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity > 10)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity <= 10 && p.StockQuantity > 0)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity == 0)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetHighlyRatedProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews)  // Include reviews for average calculation
                .Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Score) >= 4.5)  // Filter for high ratings
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLowestRatedProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews)  // Include reviews for average calculation
                .Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Score) >= 1 && p.Reviews.Average(r => r.Score) <= 2)  // Filter for low ratings
                .ToListAsync();
        }

        // Task 1: Filter Products Based on Review Count
        public async Task<IList<Product>> GetProductsByMinimumReviewCountAsync(int minReviews)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews)
                .Where(p => p.Reviews.Count >= minReviews)
                .ToListAsync();
        }

        // Task 2: Return Products with Highest Average Rating but Minimum Number of Reviews
        public async Task<IList<Product>> GetTopRatedProductsByReviewCountAsync(int minReviews)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews)
                .Where(p => p.Reviews.Count >= minReviews && p.Reviews.Average(r => r.Score) > 4)
                .ToListAsync();
        }

        // Task 3: Get Products with No Reviews
        public async Task<IList<Product>> GetProductsWithNoReviewsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews)
                .Where(p => !p.Reviews.Any())
                .ToListAsync();
        }
    }
}