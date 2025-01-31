namespace Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; }
        public string Comment { get; set; }
        public int Score { get; set; } // This is the correct property for rating
        public DateTime ReviewDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}