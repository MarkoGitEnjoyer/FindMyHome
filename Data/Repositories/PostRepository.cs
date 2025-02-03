using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PostRepository
    {
        private readonly CustomDbContext<Post> _ctx;

        public PostRepository(CustomDbContext<Post> ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(int threadId)
        {
            //var posts = await _ctx.Entity
            //    .Include(p => p.user)
            //    .Include(p => p.thread)
            //    .Where(p => p.thread.thread_id == threadId)
            //    .OrderBy(p => p.post_creationdate)
            //    .ToListAsync();


            //var groupedPosts = posts.GroupBy(p => p.post_replyuser);


            //var orderedPosts = new List<Post>();
            //foreach (var group in groupedPosts)
            //{
            //    orderedPosts.AddRange(group.OrderBy(p => p.post_creationdate));
            //}

            //return orderedPosts;
            var posts = await _ctx.Entity
        .Include(p => p.user)
        .Include(p => p.thread)
        .Where(p => p.thread.thread_id == threadId)
        .OrderBy(p => p.post_creationdate)
        .ToListAsync();

            foreach (var post in posts.Where(p => p.post_replyuser.HasValue))
            {
                post.post = await _ctx.Entity.FirstOrDefaultAsync(p => p.post_id == post.post_replyuser);
            }

            return posts;
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            var post = await _ctx.Entity
                .Include(p => p.user)
                .Include(p => p.thread)
                .FirstOrDefaultAsync(p => p.post_id == id);

            return post;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            _ctx.Entity.Add(post);
            await _ctx.SaveChangesAsync();
            return post;
        }

        public async Task UpdatePostAsync(Post post)
        {
            _ctx.Entry(post).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            _ctx.Entity.Remove(post);
            await _ctx.SaveChangesAsync();
        }
    }
}