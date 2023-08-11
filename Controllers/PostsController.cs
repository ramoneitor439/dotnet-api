using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Context;
using SocialNetwork.Dto;
using SocialNetwork.Filters;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers
{
    [ApiController]
    [Route("api/v1/post")]
    public class PostsController : ControllerBase
    {
        protected readonly PostsService _postsService;
        public PostsController(DataBaseContext _context)
            => _postsService = new(_context);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> Get()
            => Ok(await _postsService.GetAll());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<PostDto>> Get(Guid id)
            => Ok(await _postsService.Get(id));

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post(Post post)
        {
            await _postsService.Add(post);
            return CreatedAtAction(nameof(Post), post);
        }

        [HttpPut("{id:Guid}")]
        [Authorize]
        [OwnsPost]
        public async Task<ActionResult> Put(Guid id, Post post)
        {
            await _postsService.Update(id, post);
            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [Authorize]
        [OwnsPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _postsService.Delete(id);
            return NoContent();
        }
        
    }
}
