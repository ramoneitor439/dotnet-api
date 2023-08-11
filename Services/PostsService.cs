using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Dto;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class PostsService
    {
        protected readonly DataBaseContext _context;

        public PostsService(DataBaseContext context)
            => _context = context;

        public async Task<IEnumerable<PostDto>> GetAll()
        {
            var query = await (from post in _context.Set<Post>()
                               join user in _context.Set<User>()
                                   on post.UserId equals user.Id
                               orderby user.Id
                               select new PostDto
                               {
                                   Id = post.Id,
                                   Content = post.Content,
                                   Title = post.Title,
                                   User = new UserSimpleDto
                                   {
                                       Id = user.Id,
                                       Email = user.Email,
                                       UserName = user.UserName
                                   }
                               }).ToListAsync();
                             
                               
            return query;
        }

        public async Task<PostDto?> Get(Guid id)
        {
            var query = await (
                from post in _context.Set<Post>()
                join user in _context.Set<User>()
                    on post.UserId equals user.Id
                where post.Id == id
                select new PostDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    Title = post.Title,
                    User = new UserSimpleDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName
                    }
                }).FirstOrDefaultAsync();

            Console.WriteLine(query);
            return query;    
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Guid id, Post post)
        {
            if(post.Id != id)
                return false;

            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task Delete(Guid id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

    }
}
