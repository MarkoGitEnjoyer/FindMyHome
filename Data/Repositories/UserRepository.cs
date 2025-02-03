

using Data.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository
    {
        private readonly CustomDbContext<User> _ctx;
        public UserRepository(CustomDbContext<User> ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _ctx.Entity.ToListAsync();
            return users;
        }
        public async Task<User> GetUsersByIdAsync(int id)
        {
            return await _ctx.Entity.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (_ctx.Entity.Any(u => u.user_phone == user.user_phone))
            {
                Console.WriteLine("Phone already exists");
                return null;
            }
            else
            {
                try
                {
                    var query = $"INSERT INTO Users (user_fullname, user_phone, user_picture, user_isadmin, user_password) " +
                                $"VALUES ('{user.user_fullname}', '{user.user_phone}', @user_picture, {user.user_isadmin}, '{password}')";

                    await _ctx.Database.ExecuteSqlRawAsync(query,
                        new MySqlParameter("@user_picture", user.user_picture));

                    return user;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating user: {ex.Message}");
                    throw;
                }
            }
        }
        public async Task UpdateUserPasswordAsync(User user, string password)
        {
            var query = $"UPDATE Users SET user_fullname = '{user.user_fullname}', " +
                $"user_phone = '{user.user_phone}', user_picture = @user_picture, " +
                $"user_isadmin = {user.user_isadmin}";

            if (!string.IsNullOrEmpty(password))
            {
                query += $", user_password = '{password}'";
            }

            query += $" WHERE user_id = {user.user_id}";

            await _ctx.Database.ExecuteSqlRawAsync(query,
                new MySqlParameter("@user_picture", user.user_picture));
        }
        public async Task UpdateUserAsync(User user)
        {
            _ctx.Entry(user).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(User user)
        {
            _ctx.Entity.Remove(user);
            await _ctx.SaveChangesAsync();
        }
        public async Task<User> AuthenticateAsync(string phone, string enteredPassword)
        {
            try
            {
                var query = $"SELECT * FROM users WHERE user_phone = '{phone}' AND user_password = '{enteredPassword}'";
                var user = await _ctx.Entity.FromSqlRaw(query).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
                return null;
            }
        }

    }
}
