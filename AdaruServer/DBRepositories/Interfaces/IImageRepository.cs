using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Interfaces
{
    public interface IImageRepository
    {
        Task<Image> GetImage(int imageId);
        Task AddImage(Image image);
    }
}
