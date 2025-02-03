using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Models
{
    public class CustomDbContext<T> : DbContext where T : class
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CustomDbContext()
        {
        }

        public CustomDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DbSet<T> Entity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }
    }
}