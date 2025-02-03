using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CategoryRepository
    {
        private readonly CustomDbContext<Category> _ctx;

        public CategoryRepository(CustomDbContext<Category> ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _ctx.Entity.ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _ctx.Entity.FindAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _ctx.Entity.Add(category);
            await _ctx.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _ctx.Entry(category).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _ctx.Entity.Remove(category);
            await _ctx.SaveChangesAsync();
        }
    }
}