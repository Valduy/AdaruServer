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
        private IWebHostEnvironment _environment;
        private IImageRepository _imageRepository;

        public ImageService(
            IWebHostEnvironment environment,
            IImageRepository imageRepository)
        {
            _environment = environment;
            _imageRepository = imageRepository;
        }

        public async Task<Image> AddImageAsync(string login, string image, string coerce = "")
        {
            var path = Path.Combine(_environment.WebRootPath, "Images", login);
            var imageName = Regex.Replace(login + DateTime.Now + coerce, @"\.|\s|:", (_) => "") + ".jpg";
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
    }
}
