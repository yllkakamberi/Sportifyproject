﻿namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }

        // Navigation Properties
        public int StockQuantity { get; set; }

        public ProductType? ProductType { get; set; }
        public ProductBrand? ProductBrand { get; set; }

        // Add this navigation property for reviews
        public List<Review> Reviews { get; set; } = new List<Review>();  // Navigation property for reviews
    }
}
