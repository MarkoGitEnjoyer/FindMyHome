
using Data.Models;
using Data.Repositories;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<CustomDbContext<User>>();
            builder.Services.AddTransient<CustomDbContext<Animal>>();
            builder.Services.AddTransient<CustomDbContext<Category>>();
            builder.Services.AddTransient<CustomDbContext<Data.Models.Thread>>();
            builder.Services.AddTransient<CustomDbContext<Post>>();
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<AnimalRepository>();
            builder.Services.AddTransient<CategoryRepository>();
            builder.Services.AddTransient<ThreadRepository>();
            builder.Services.AddTransient<PostRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
