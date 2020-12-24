using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdaruServer.Controllers
{
    public class TagController : Controller
    {
        private ITagRepository _tagRepository;
        private IMapper _mapper;

        public TagController(
            ITagRepository tagRepository, 
            IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }


    }
}
