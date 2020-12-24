using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace AdaruServer.Services.Implementation
{
    public interface IImageService
    {
        public Task<Image> AddImageAsync(string login, string image);
        public Task<List<Image>> AddImagesAsync(string login, string[] images);
        public void DeleteImage(Image image);
    }
}
