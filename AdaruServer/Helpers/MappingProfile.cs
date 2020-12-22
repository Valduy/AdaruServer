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
        }
    }
}
