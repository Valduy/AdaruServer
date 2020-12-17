using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Repositories
{
    public class ImageRepository : BaseRepository, IImageRepository
    {
        public ImageRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Image> GetImage(int imageId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return await context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
        }

        public async Task AddImage(Image image)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Images.AddAsync(image);
            await context.SaveChangesAsync();
        }
    }
}
