﻿using Core.Entities;
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

        // Task 2: Get Products with Stock Greater Than 10
        public async Task<IEnumerable<Product>> GetProductsWithSufficientStockAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity > 10)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();  // ToListAsync() ensures it's async
        }


        // Task 3: Get Low Stock Products (Stock ≤ 10)
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity <= 10 && p.StockQuantity > 0)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();  // ToListAsync() ensures it's async
        }


        // Task 4: Get Out-of-Stock Products (Stock == 0)
        public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity == 0)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();  // ToListAsync() ensures it's async
        }

        }
    }
