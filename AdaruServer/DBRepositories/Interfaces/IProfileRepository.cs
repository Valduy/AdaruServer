using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> GetProfile(int profileId);
        Task AddProfile(Profile profile);
        Task AddImage(Image image);
        Task AddImages(IEnumerable<Image> images);
        Task RemoveImage(Image image);
    }
}
