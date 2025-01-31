using Core.Entities;

public interface IReviewRepository
{
    Task<Review> AddAsync(Review review); // Renamed from AddReviewAsync
    Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
    Task<Review> GetReviewByIdAsync(int reviewId);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
}