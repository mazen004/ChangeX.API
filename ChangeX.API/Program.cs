using ChangeX.BLL.Services;
using ChangeX.DAL.Database;
using ChangeX.BLL.Interfaces;
// using ChangeX.BLL.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
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

            builder.Services.AddTransient<IUserServices, UserServices>();
            builder.Services.AddTransient<ICRService, CRService>();

            //builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddCors();
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
