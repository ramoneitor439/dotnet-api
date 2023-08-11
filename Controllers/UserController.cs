using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Context;
using SocialNetwork.Dto;
using SocialNetwork.Filters;
using SocialNetwork.Identity;
using SocialNetwork.Models;
using SocialNetwork.Services;


namespace SocialNetwork.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        protected readonly UserService _userService;
        public UserController(DataBaseContext _context)
            => _userService = new(_context);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
            => Ok(await _userService.GetAll());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var user = await _userService.Get(id);
            return (user is null)? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            await _userService.Add(user);
            return CreatedAtAction(nameof(Get), user);
        }

        [HttpPut("{id:Guid}")]
        [Authorize]
        [OwnsAccount]
        public async Task<ActionResult> Update(Guid id, User user) 
        {
            try
            {
                var ok = await _userService.Update(id, user);
                if (!ok)
                    return BadRequest();
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
            return CreatedAtAction(nameof(Get), user);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [OwnsAccount]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userService.Remove(id);
            return NoContent();
        }



    }
}
