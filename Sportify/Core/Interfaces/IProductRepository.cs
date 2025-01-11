using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IList<ProductBrand>> GetProductBrandsAsync();
        Task<IList<ProductType>> GetProductTypesAsync();
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName);
        Task<IEnumerable<Product>> GetProductsByTypeAsync(string typeName);

        // Add the new method to search products by name
        Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
    }
}
