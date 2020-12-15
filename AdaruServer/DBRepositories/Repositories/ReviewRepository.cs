using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Repositories
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        public ReviewRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Review> GetReview(int reviewId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task AddReview(Review review)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetReviewsAboutClient(int clientId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Reviews.Where(r => r.IdTarget == clientId).ToListAsync();
        }

        public async Task<List<Review>> GetClientReviews(int clientId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Reviews.Where(r => r.IdSource == clientId).ToListAsync();
        }
    }
}
