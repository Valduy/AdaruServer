using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Extensions;
using AdaruServer.Services.Implementation;
using AdaruServer.ViewModels;
using AutoMapper;
using DBRepository;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Profile = Models.Profile;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private IProfileRepository _profileRepository;
        private IClientRepository _clientRepository;
        private IImageRepository _imageRepository;
        private IImageService _imageService;
        private IMapper _mapper;

        public ProfileController(
            IProfileRepository profileRepository,
            IClientRepository clientRepository,
            IImageRepository imageRepository,
            IImageService imageService,
            IMapper mapper)
        {
            _profileRepository = profileRepository;
            _clientRepository = clientRepository;
            _imageRepository = imageRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        // api/profile/add
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddProfile([FromBody]string resume)
        {
            try
            {
                var userId = User.GetName();
                var profile = _mapper.Map<Models.Profile>(resume);
                profile.IdClient = int.Parse(userId);
                await _profileRepository.AddProfile(profile);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }

            return Ok();
        }

        // api/profile/my
        [Authorize]
        [HttpGet("my")]
        public async Task<ProfileViewModel> GetProfile()
        {
            return await GetProfileViewModelAsync(await _profileRepository.GetProfile(int.Parse(User.GetName())));
        }

        // api/profile/concrete?id=1
        [HttpGet("concrete")]
        public async Task<ProfileViewModel> GetProfile(int id)
        {
            return await GetProfileViewModelAsync(await _profileRepository.GetProfile(id));
        }

        // api/profile/update
        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateProfile([FromBody]string resume)
        {
            try
            {
                var userId = User.GetName();
                var profile = await _profileRepository.GetProfile(int.Parse(userId));
                profile.Resume = resume;
                await _profileRepository.UpdateProfile(profile);
                _mapper.Map<ProfileViewModel>(profile);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok();
        }

        // api/profile/images/add
        [Authorize]
        [HttpPost("images/add")]
        public async Task AddImages([FromBody]IEnumerable<ImageViewModel> images)
        {
            var client = await _clientRepository.GetClient(int.Parse(User.GetName()));
            var newImages = await _imageService
                .AddImagesAsync(client.Login, images.Select(i => i.Image));
            await _profileRepository.AddImages(client.Id, newImages);

            var imagesWithTags = newImages
                .Zip(images, (ni, i) => new
                {
                    ni.Id, 
                    i.Tags
                });

            try
            {
                foreach (var it in imagesWithTags)
                {
                    await _profileRepository.AddTagsToImage(it.Id, it.Tags);
                }
            }
            catch (RepositoryException ex)
            {
                BadRequest(new { message = ex.Message });
            }
        }

        // api/profile/images/add
        [Authorize]
        [HttpDelete("image/delete")]
        public async Task DeleteImage(int id)
        {
            var userId = int.Parse(User.GetName());
            var image = await _imageRepository.GetImage(id);
            await _profileRepository.DeleteImage(userId, image);
            await _imageRepository.DeleteImage(image);
            _imageService.DeleteImage(image);
        }

        // api/profile/add/tags
        [Authorize]
        [HttpPost("add/tags")]
        public async Task<IActionResult> AddTagsToImage(int id, [FromBody]IEnumerable<string> tags)
        {
            try
            {
                var userId = int.Parse(User.GetName());
                var image = await _profileRepository.GetImage(userId, id);
                await _profileRepository.AddTagsToImage(image, tags);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }

            return Ok();
        }

        // api/task/image/get
        [HttpGet("image/get")]
        public async Task<ImageViewModel> GetImage(int id)
        {
            var image = await _imageRepository.GetImage(id);
            var result = new ImageViewModel
            {
                Image = await _imageService.GetImage(image),
                Tags = (await _profileRepository.GetImageTags(image.Id)).Select(t => t.Name)
            };
            return result;
        }

        // api/task/delete/tags?id=1
        [Authorize]
        [HttpDelete("delete/tags")]
        public async Task<IActionResult> DeleteImageTags(int id, [FromBody] IEnumerable<string> tags)
        {
            var userId = int.Parse(User.GetName());
            var image = await _profileRepository.GetImage(userId, id);

            if (image == null)
            {
                return BadRequest(new { message = "Попытка изменить не своё изображение." });
            }

            await _profileRepository.DeleteImageTags(image, tags);
            return Ok();
        }

        private async Task<ProfileViewModel> GetProfileViewModelAsync(Profile profile)
        {
            var result = _mapper.Map<ProfileViewModel>(profile);
            result.Images = (await _profileRepository.GetProfileImages(profile)).Select(i => i.Id);
            return result;
        }
    }
}
