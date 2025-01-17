using System;

namespace Core.Entities
{
    public class Review : BaseEntity
    {
        public string ReviewText { get; set; }

        public int Score { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
