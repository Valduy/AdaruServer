using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet("tags")]
        public async Task<IEnumerable<string>> GetAllTags()
        {
            var tags = await _tagRepository.GetAllTags();
            return tags.Select(t => t.Name);
        }
    }
}
