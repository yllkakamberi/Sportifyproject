using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        // Product-related methods
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName);
        Task<IEnumerable<Product>> GetProductsByTypeAsync(string typeName);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        // Stock-related methods
        Task<IEnumerable<Product>> GetProductsWithSufficientStockAsync();  // Stock > 10
        Task<IEnumerable<Product>> GetLowStockProductsAsync();  // Stock <= 10
        Task<IEnumerable<Product>> GetOutOfStockProductsAsync();  // Stock == 0

        // Rating-related methods
        Task<IEnumerable<Product>> GetHighlyRatedProductsAsync();  // Average rating >= 4
        Task<IEnumerable<Product>> GetLowestRatedProductsAsync();  // Average rating <= 2

        // ProductBrand and ProductType methods
        Task<IList<ProductBrand>> GetProductBrandsAsync();
        Task<IList<ProductType>> GetProductTypesAsync();

        // Part 6: Advanced Review Filtering and Aggregation
        Task<IList<Product>> GetProductsByMinimumReviewCountAsync(int minReviews); // Task 1
        Task<IList<Product>> GetTopRatedProductsByReviewCountAsync(int minReviews); // Task 2
        Task<IList<Product>> GetProductsWithNoReviewsAsync(); // Task 3
    }
}