﻿using System.Collections.Generic;
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
        Task RemoveImage(int profileId, Image image);
        Task UpdateProfile(Profile profile);
    }
}
