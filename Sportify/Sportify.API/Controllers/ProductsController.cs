using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportify.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SportifyDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IReviewRepository _reviewRepository;

        public ProductsController(SportifyDbContext context, ILogger<ProductsController> logger, IProductRepository productRepository, IReviewRepository reviewRepository)
        {
            _context = context;
            _logger = logger;
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
        }

        // Existing endpoints (unchanged)

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

        // Review endpoints (unchanged)

        [HttpPost("{productId}/reviews")]
        public async Task<ActionResult<Review>> CreateReview(int productId, [FromBody] Review newReview)
        {
            if (newReview == null)
            {
                return BadRequest("Review cannot be null.");
            }

            try
            {
                var product = await _context.Set<Product>().FindAsync(productId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                newReview.ProductId = productId;
                await _reviewRepository.AddAsync(newReview);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductById), new { id = productId }, newReview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new review.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{productId}/reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForProduct(int productId)
        {
            try
            {
                var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);

                if (reviews == null || !reviews.Any())
                {
                    return NotFound("No reviews found for this product.");
                }

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reviews for the product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("reviews/{reviewId}")]
        public async Task<ActionResult<Review>> GetReviewById(int reviewId)
        {
            try
            {
                var review = await _reviewRepository.GetReviewByIdAsync(reviewId);

                if (review == null)
                {
                    return NotFound("Review not found.");
                }

                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching the review.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("reviews/{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] Review updatedReview)
        {
            if (updatedReview == null)
            {
                return BadRequest("Review cannot be null.");
            }

            try
            {
                var existingReview = await _reviewRepository.GetReviewByIdAsync(reviewId);

                if (existingReview == null)
                {
                    return NotFound("Review not found.");
                }

                existingReview.Score = updatedReview.Score; // Use Score instead of Rating
                existingReview.Comment = updatedReview.Comment;

                await _reviewRepository.UpdateAsync(existingReview);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the review.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                var review = await _reviewRepository.GetReviewByIdAsync(reviewId);

                if (review == null)
                {
                    return NotFound("Review not found.");
                }

                await _reviewRepository.DeleteAsync(review);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the review.");
                return StatusCode(500, "Internal server error");
            }
        }

        // Stock-related endpoints (unchanged)

        [HttpGet("sufficient-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsWithSufficientStock()
        {
            try
            {
                var products = await _productRepository.GetProductsWithSufficientStockAsync();

                if (products == null || !products.Any())
                {
                    return NotFound("No products with sufficient stock found.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products with sufficient stock.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetLowStockProducts()
        {
            try
            {
                var products = await _productRepository.GetLowStockProductsAsync();

                if (products == null || !products.Any())
                {
                    return NotFound("No low stock products found.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching low stock products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("out-of-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetOutOfStockProducts()
        {
            try
            {
                var products = await _productRepository.GetOutOfStockProductsAsync();

                if (products == null || !products.Any())
                {
                    return NotFound("No out-of-stock products found.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching out-of-stock products.");
                return StatusCode(500, "Internal server error");
            }
        }

        // Rating-related endpoints (unchanged)

        [HttpGet("highly-rated")]
        public async Task<IActionResult> GetHighlyRatedProducts()
        {
            var products = await _productRepository.GetHighlyRatedProductsAsync();
            return Ok(products);
        }

        [HttpGet("lowest-rated")]
        public async Task<IActionResult> GetLowestRatedProducts()
        {
            var products = await _productRepository.GetLowestRatedProductsAsync();
            return Ok(products);
        }

        // New endpoints for Part 6: Advanced Review Filtering and Aggregation

        [HttpGet("minimum-reviews/{minReviews}")]
        public async Task<IActionResult> GetProductsByMinimumReviewCount(int minReviews)
        {
            try
            {
                var products = await _productRepository.GetProductsByMinimumReviewCountAsync(minReviews);

                if (products == null || !products.Any())
                {
                    return NotFound($"No products found with at least {minReviews} reviews.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products by minimum review count.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("top-rated/{minReviews}")]
        public async Task<IActionResult> GetTopRatedProductsByReviewCount(int minReviews)
        {
            try
            {
                var products = await _productRepository.GetTopRatedProductsByReviewCountAsync(minReviews);

                if (products == null || !products.Any())
                {
                    return NotFound($"No highly-rated products found with at least {minReviews} reviews.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching top-rated products by review count.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("no-reviews")]
        public async Task<IActionResult> GetProductsWithNoReviews()
        {
            try
            {
                var products = await _productRepository.GetProductsWithNoReviewsAsync();

                if (products == null || !products.Any())
                {
                    return NotFound("No products without reviews found.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products with no reviews.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}