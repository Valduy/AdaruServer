using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Extensions;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Repositories
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
            return context.Reviews.FirstOrDefault(r => r.Id == reviewId);
        }

        public async Task AddReview(Review review)
        {
            try
            {
                await using var context = ContextFactory.CreateDbContext(ConnectionString);
                await context.Reviews.AddAsync(review);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException innerException)
                {
                    switch (innerException.SqlState)
                    {
                        case PgsqlErrors.RaiseException:
                            throw new RepositoryException(innerException.MessageText);
                        case PgsqlErrors.UniqueViolation:
                            throw new RepositoryException("Отзыв об этом пользователе уже оставлен.");
                    }
                }

                throw;
            }
        }

        public async Task<List<Review>> GetReviewsAboutClient(int clientId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Reviews.Where(r => r.IdTarget == clientId).ToListAsyncSafe();
        }

        public async Task<List<Review>> GetClientReviews(int clientId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Reviews.Where(r => r.IdSource == clientId).ToListAsyncSafe();
        }
    }
}
