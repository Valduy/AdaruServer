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
        Task AddImage(int profileId, Image image);
        Task AddImages(int profileId, IEnumerable<Image> images);
        Task DeleteImage(int profileId, Image image);
        Task DeleteImage(int profileId, int imageId);
        Task UpdateProfile(Profile profile);
        Task<List<Image>> GetProfileImages(Profile profile);
        Task<Image> GetImage(int profileId, int imageId);
        Task<List<Tag>> GetImageTags(int imageId);
        Task AddTagsToImage(Image image, IEnumerable<string> tags);
        Task DeleteImageTags(Image image, IEnumerable<string> tags);
    }
}
