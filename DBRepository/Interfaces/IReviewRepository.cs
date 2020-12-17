using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetReview(int reviewId);
        Task AddReview(Review review);
        Task<List<Review>> GetReviewsAboutClient(int clientId);
        Task<List<Review>> GetClientReviews(int clientId);
    }
}
