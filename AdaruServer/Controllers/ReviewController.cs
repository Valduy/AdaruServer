using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Extensions;
using AdaruServer.ViewModels;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using IMapper = AutoMapper.IMapper;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private IReviewRepository _reviewRepository;
        private IClientRepository _clientRepository;
        private IRoleRepository _roleRepository;
        private IMapper _mapper;

        public ReviewController(
            IReviewRepository reviewRepository, 
            IClientRepository clientRepository,
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _clientRepository = clientRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        // api/review/reviews?id=1
        [HttpGet("reviews")]
        public async Task<List<ReviewViewModel>> GetReviews(int id)
        {
            var reviews = await _reviewRepository.GetReviewsAboutClient(id);
            var result = new List<ReviewViewModel>();

            foreach (var r in reviews)
            {
                var reviewWm = _mapper.Map<ReviewViewModel>(r);
                var client = await _clientRepository.GetClient(r.IdSource);
                var clientVm = _mapper.Map<ClientViewModel>(client);
                clientVm.Role = (await _roleRepository.GetUserRole(client.IdRole)).Role;
                reviewWm.Sender = clientVm;
                result.Add(reviewWm);
            }

            return result;
        }

        // api/review/my
        [Authorize]
        [HttpGet("reviews/my")]
        public async Task<List<ReviewViewModel>> GetMyReviews()
        {
            var reviews = await _reviewRepository.GetClientReviews(int.Parse(User.GetName()));
            return await CreateReviewViewModels(reviews);
        }

        // api/review/me
        [Authorize]
        [HttpGet("reviews/me")]
        public async Task<List<ReviewViewModel>> GetReviewsAboutMe()
        {
            var reviews = await _reviewRepository.GetReviewsAboutClient(int.Parse(User.GetName()));
            return await CreateReviewViewModels(reviews);
        }

        private async Task<List<ReviewViewModel>> CreateReviewViewModels(List<Review> reviews)
        {
            var result = new List<ReviewViewModel>();

            foreach (var r in reviews)
            {
                var reviewWm = _mapper.Map<ReviewViewModel>(r);
                var client = await _clientRepository.GetClient(r.IdSource);
                var clientVm = _mapper.Map<ClientViewModel>(client);
                clientVm.Role = (await _roleRepository.GetUserRole(client.IdRole)).Role;
                reviewWm.Sender = clientVm;
                result.Add(reviewWm);
            }

            return result;
        }
    }
}
