using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> GetProfile(int profileId);
        Task AddProfile(Profile profile);
        Task AddImage(Profile profile, Image image);
        Task AddImages(Profile profile, IEnumerable<Image> images);
        Task RemoveImage(Profile profile, Image image);
    }
}
