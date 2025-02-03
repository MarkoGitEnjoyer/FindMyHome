using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;
using Thread = Data.Models.Thread;

namespace Data.Repositories

{
    public class ThreadRepository
    {
        private readonly CustomDbContext<Thread> _ctx;

        public ThreadRepository(CustomDbContext<Thread> ctx)
        {
            _ctx = ctx;
        }
 
        public async Task<IEnumerable<Thread>> GetThreadsAsync()
        {
            var threads = await _ctx.Entity.Include(t => t.user).Include(t => t.category).ToListAsync();
            return threads;
        }

        public async Task<Data.Models.Thread> GetThreadByIdAsync(int id)
        {
            var thread = await _ctx.Entity
                .Include(t => t.user)
                .Include(t => t.category)
                .FirstOrDefaultAsync(t => t.thread_id == id);

            return thread;
        }
        public async Task<IEnumerable<Thread>> GetThreadsByCategoryAsync(int categoryId)
        {
            var threadsInCategory = await _ctx.Entity
                .Include(t => t.user)
                .Include(t => t.category)
                .Where(t => t.category.category_id == categoryId)
                .ToListAsync();

            return threadsInCategory;
        }

        public async Task<Thread> CreateThreadAsync(Thread thread)
        {
            _ctx.Entity.Add(thread);
            await _ctx.SaveChangesAsync();
            return thread;
        }

        public async Task UpdateThreadAsync(Thread thread)
        {
            _ctx.Entry(thread).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteThreadAsync(Thread thread)
        {
            _ctx.Entity.Remove(thread);
            await _ctx.SaveChangesAsync();
        }
    }
}