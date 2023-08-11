using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialNetwork.Context;
using SocialNetwork.Dto;
using SocialNetwork.Models;
using System.Runtime.CompilerServices;

namespace SocialNetwork.Services
{
    public class UserService
    {
        protected readonly DataBaseContext _context;
        public UserService(DataBaseContext _db) => _context = _db;

        public async Task<IEnumerable<UserDto>> GetAll()
        {

            var query = await (
                from user in _context.Set<User>()
                join role in _context.Set<Role>()
                on user.RoleId equals role.Id
                orderby user.UserName
                select new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Posts = (from post in _context.Set<Post>()
                             where post.UserId == user.Id
                             select new PostSimpleDto
                             {
                                 Id = post.Id,
                                 Title = post.Title,
                                 Content = post.Content,
                             }).ToList(),
                    Role = role.Name
                }
            ).ToListAsync();
            return query;
        }

        public async Task<UserDto?> Get(Guid id)
        {
            var query = await (
                from user in _context.Set<User>()
                where user.Id == id
                select new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Posts = (from post in _context.Set<Post>()
                             where post.UserId == user.Id
                             select new PostSimpleDto
                             {
                                 Id = post.Id,
                                 Title = post.Title,
                                 Content = post.Content,
                             }).ToList(),
                    Role = (from role in _context.Set<Role>()
                            where role.Id == user.RoleId
                            select role.Name).First()
                }
            ).FirstOrDefaultAsync();
            
            return query;
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Guid id, User user)
        {
            if (id != user.Id)
                return false;

            _context.Entry(user).State = EntityState.Modified;
            try 
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(await _context.Users.FindAsync(id) == null)
                {
                    return false;
                }
                throw;
            }
            return true;
        }

        public async Task Remove(Guid id)
        {
            var user = _context.Users.Find(id);
            if(user != null) { 
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

    }
        
}
