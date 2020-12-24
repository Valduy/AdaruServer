using System.Collections.Generic;
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
    public class ImageRepository : BaseRepository, IImageRepository
    {
        public ImageRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Image> GetImage(int imageId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.Images.FirstOrDefault(i => i.Id == imageId);
        }

        public async Task AddImage(Image image)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Images.AddAsync(image);
            await context.SaveChangesAsync();
        }

        public async Task DeleteImage(int imageId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            context.Images.Remove(context.Images.First(i => i.Id == imageId));
            await context.SaveChangesAsync();
        }

        public async Task DeleteImage(Image image) => await DeleteImage(image.Id);
    }
}
