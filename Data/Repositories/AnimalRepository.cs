using Data.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AnimalRepository 
    {
        private readonly CustomDbContext<Animal> _ctx;
        public AnimalRepository(CustomDbContext<Animal> ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<Animal>> GetAnimalsAsync()
        {
            var animals = await _ctx.Entity.Include(a => a.user).ToListAsync();
            return animals;
        }
        public async Task<List<string>> GetDistinctTypesAsync()
        {
            return await _ctx.Entity.Select(a => a.animal_type).Distinct().ToListAsync();
        }

        public async Task<List<string>> GetDistinctBreedsAsync()
        {
            return await _ctx.Entity.Select(a => a.animal_breed).Distinct().ToListAsync();
        }
        public async Task<List<string>> GetDistinctBreedsAsync(string type)
        {
            return await _ctx.Entity
             .Where(a => a.animal_type == type)
             .Select(a => a.animal_breed)
             .Distinct()
             .ToListAsync();
        }
        public async Task<Animal> GetAnimalsByIdAsync(int id)
        {
            var animal = await _ctx.Entity
                .Include(a => a.user)
                .FirstOrDefaultAsync(a => a.animal_id == id);

            return animal;
        }
        public async Task<Animal> CreateAnimalAsync(Animal animal)
        {
            try
            {
                var query = $"INSERT INTO animals (animal_status, user_id, animal_picture, animal_breed, animal_description, " +
                            $"animal_longtitude, animal_latitude, animal_date, animal_type) " +
                            $"VALUES ('{animal.animal_status}', {animal.user_id}, @animal_picture, " +
                            $"'{animal.animal_breed}', '{animal.animal_description}', '{animal.animal_longtitude}', " +
                            $"'{animal.animal_latitude}', '{animal.animal_date.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                            $"'{animal.animal_type}')";

                await _ctx.Database.ExecuteSqlRawAsync(query,
                    new MySqlParameter("@animal_picture", animal.animal_picture));

                return animal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating animal: {ex.Message}");
                throw;
            }
        }
        public async Task UpdateAnimalAsync(Animal animal)
        {
            _ctx.Entry(animal).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteAnimalAsync(Animal animal)
        {
            _ctx.Entity.Remove(animal);
            await _ctx.SaveChangesAsync();
        }
    }
}
