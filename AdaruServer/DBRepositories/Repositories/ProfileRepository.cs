using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Repositories
{
    public class ProfileRepository : BaseRepository, IProfileRepository
    {
        public ProfileRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public Task<Profile> GetProfile(int profileId)
        {
            throw new NotImplementedException();
        }

        public Task AddProfile(Profile profile)
        {
            throw new NotImplementedException();
        }

        public Task AddImage(Image image)
        {
            throw new NotImplementedException();
        }

        public Task AddImages(IEnumerable<Image> images)
        {
            throw new NotImplementedException();
        }

        public Task RemoveImage(Image image)
        {
            throw new NotImplementedException();
        }
    }
}
