﻿using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sportify.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SportifyDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductsController(SportifyDbContext context, ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            _context = context;
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                var products = await _context.Set<Product>()
                                              .AsNoTracking()
                                              .Include(p => p.ProductType)
                                              .Include(p => p.ProductBrand)
                                              .ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _context.Set<Product>()
                                             .AsNoTracking()
                                             .Include(p => p.ProductType)
                                             .Include(p => p.ProductBrand)
                                             .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching product with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product newProduct)
        {
            if (newProduct == null)
            {
                return BadRequest("Product cannot be null.");
            }

            try
            {
                await _context.Set<Product>().AddAsync(newProduct);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product {ProductId} created successfully.", newProduct.Id);

                return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest("Updated product cannot be null.");
            }

            try
            {
                var product = await _context.Set<Product>().FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                product.Name = updatedProduct.Name;
                product.Description = updatedProduct.Description;
                product.Price = updatedProduct.Price;
                product.PictureUrl = updatedProduct.PictureUrl;
                product.ProductTypeId = updatedProduct.ProductTypeId;
                product.ProductBrandId = updatedProduct.ProductBrandId;

                _context.Set<Product>().Update(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Set<Product>().FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                _context.Set<Product>().Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("bybrand")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByBrand([FromQuery] int productBrandId)
        {
            try
            {
                var products = await _context.Set<Product>()
                                              .AsNoTracking()
                                              .Include(p => p.ProductType)
                                              .Include(p => p.ProductBrand)
                                              .Where(p => p.ProductBrandId == productBrandId)
                                              .ToListAsync();

                if (products == null || products.Count == 0)
                {
                    return NotFound($"No products found for the brand with ID {productBrandId}.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching products for brand with ID {productBrandId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("bytype")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByType([FromQuery] int productTypeId)
        {
            try
            {
                var products = await _context.Set<Product>()
                                              .AsNoTracking()
                                              .Include(p => p.ProductType)
                                              .Include(p => p.ProductBrand)
                                              .Where(p => p.ProductTypeId == productTypeId)
                                              .ToListAsync();

                if (products == null || products.Count == 0)
                {
                    return NotFound($"No products found for the product type with ID {productTypeId}.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching products for type with ID {productTypeId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("byname")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName([FromQuery] string productName)
        {
            try
            {
                var products = await _productRepository.GetProductsByNameAsync(productName);

                if (products == null || !products.Any())
                {
                    return NotFound($"No products found for the name containing '{productName}'.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while searching for products with name containing '{productName}'.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("byprice")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            try
            {
                var products = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);

                if (products == null || !products.Any())
                {
                    return NotFound($"No products found within the price range {minPrice} to {maxPrice}.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching products in the price range {minPrice} to {maxPrice}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
