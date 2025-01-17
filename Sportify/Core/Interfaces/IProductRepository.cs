using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IList<ProductBrand>> GetProductBrandsAsync();
        Task<IList<ProductType>> GetProductTypesAsync();
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName);
        Task<IEnumerable<Product>> GetProductsByTypeAsync(string typeName);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsWithSufficientStockAsync();  // Stock > 10
        Task<IEnumerable<Product>> GetLowStockProductsAsync();  // Stock <= 10
        Task<IEnumerable<Product>> GetOutOfStockProductsAsync();  //
    }
}
