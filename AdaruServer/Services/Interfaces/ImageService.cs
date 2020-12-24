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
            var imageName = Regex.Replace(login + DateTime.Now, @"\.|\s|:", (_) => "") + Jpg;
            var imagePath = Path.Combine(path, imageName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var imageBytes = Convert.FromBase64String(image);
            await File.WriteAllBytesAsync(imagePath, imageBytes);
            var newImage = new Image { Path = imagePath };
            await _imageRepository.AddImage(newImage);
            return newImage;
        }

        public async Task<List<Image>> AddImagesAsync(string login, string[] images)
        {
            var path = Path.Combine(_environment.WebRootPath, "Images", login);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var result = new List<Image>();
            var name = Regex.Replace(login + DateTime.Now, @"\.|\s|:", (_) => "");

            for (int i = 0; i < images.Length; i++)
            {
                var imageName = name + i + Jpg;
                var imagePath = Path.Combine(path, imageName);
                var imageBytes = Convert.FromBase64String(images[i]);
                await File.WriteAllBytesAsync(imagePath, imageBytes);
                var newImage = new Image { Path = imagePath };
                await _imageRepository.AddImage(newImage);
                result.Add(newImage);
            }

            return result;
        }
    }
}
