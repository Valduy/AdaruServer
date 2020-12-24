using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IImageRepository
    {
        Task<Image> GetImage(int imageId);
        Task AddImage(Image image);
        Task DeleteImage(int imageId);
        Task<List<Tag>> GetImageTags(int imageId);
    }
}
