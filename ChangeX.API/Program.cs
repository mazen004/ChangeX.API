using ChangeX.BLL.Services;
using ChangeX.DAL.Database;
using ChangeX.BLL.Interfaces;
using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<ICRService, CRService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();


            builder.Services.AddAutoMapper(configuration =>
                configuration.CreateMap<User, UserInClientDto>());

            builder.Services.AddCors();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' was not found.");

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
               options.SwaggerEndpoint("/openapi/v1.json", "ChangeX"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
