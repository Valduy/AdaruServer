using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdaruServer.Services.Implementation;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.Services.Interfaces
{
    public class ImageService : IImageService
    {
        public const string Jpg = ".jpg";

        private IWebHostEnvironment _environment;
        private IImageRepository _imageRepository;

        public ImageService(
            IWebHostEnvironment environment,
            IImageRepository imageRepository)
        {
            _environment = environment;
            _imageRepository = imageRepository;
        }

        public async Task<Image> AddImageAsync(string login, string image)
        {
            var path = Path.Combine(_environment.WebRootPath, "Images", login);
            var imageName = GetImageName(login);
            var imagePath = Path.Combine(path, imageName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return await SaveImageAsync(imagePath, image);
        }

        public async Task<List<Image>> AddImagesAsync(string login, IEnumerable<string> images)
        {
            var path = Path.Combine(_environment.WebRootPath, "Images", login);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var result = new List<Image>();
            var name = GetImageName(login);
            int i = 0;

            foreach (var image in images)
            {
                var imageName = i + name;
                var imagePath = Path.Combine(path, imageName);
                result.Add(await SaveImageAsync(imagePath, image));
                i++;
            }

            return result;
        }

        public async Task<string> GetImage(Image image)
        {
            var bytes = await File.ReadAllBytesAsync(image.Path);
            return Convert.ToBase64String(bytes);
        }

        public void DeleteImage(Image image)
        {
            if (File.Exists(image.Path))
            {
                File.Delete(image.Path);
            }
        }

        private async Task<Image> SaveImageAsync(string path, string image)
        {
            var imageBytes = Convert.FromBase64String(image);
            await File.WriteAllBytesAsync(path, imageBytes);
            var newImage = new Image { Path = path };
            await _imageRepository.AddImage(newImage);
            return newImage;
        }

        private string GetImageName(string login) 
            => Regex.Replace(login + DateTime.Now, @"\.|\s|:", (_) => "") + Jpg;
    }
}
