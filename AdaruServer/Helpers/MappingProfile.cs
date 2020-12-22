using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.ViewModels;
using Models;
using Profile = AutoMapper.Profile;

namespace AdaruServer.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationViewModel, Client>();
            CreateMap<AddTaskViewModel, Models.Task>();
            CreateMap<Models.Task, TaskViewModel>();
            CreateMap<Client, ClientViewModel>();
            CreateMap<Models.Profile, ProfileViewModel>();
            CreateMap<Review, ReviewViewModel>();
            CreateMap<AddReviewViewModel, Review>();
            CreateMap<Message, MessageViewModel>();
            CreateMap<string, Tag>()
                .ForMember(m => m.Name, opt => opt.MapFrom(m => m));
            CreateMap<string, Models.Profile>()
                .ForMember(m => m.Resume, opt => opt.MapFrom(m => m));
        }
    }
}
