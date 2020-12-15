using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetReview(int reviewId);
        Task AddReview(Review review);
        Task<List<Review>> GetReviewsAboutClient(int clientId);
        Task<List<Review>> GetClientReviews(int clientId);
    }
}
