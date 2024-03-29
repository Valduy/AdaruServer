﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace AdaruServer.Services.Implementation
{
    public interface IImageService
    {
        public Task<Image> AddImageAsync(string login, string image);
        public Task<List<Image>> AddImagesAsync(string login, IEnumerable<string> images);
        public Task<string> GetImage(Image image);
        public void DeleteImage(Image image);
    }
}
