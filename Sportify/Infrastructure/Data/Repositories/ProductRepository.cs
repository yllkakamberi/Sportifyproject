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

        // Add the method to search products by name
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
    }
}
