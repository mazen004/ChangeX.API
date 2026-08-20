using ChangeX.BLL.Interfaces;
using ChangeX.BLL.Mapping;
using ChangeX.BLL.Services;
using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChangeX.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["AppSettings:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
                        ValidateIssuerSigningKey = true
                    };

                });

            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IProjectService, ProjectService>();

            builder.Services.AddScoped<IClientServices, ClientServices>();
            builder.Services.AddScoped<ICRServices, CRService>();
            builder.Services.AddScoped<IDetailServices, DetailServices>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingClientProfile>();
                cfg.AddProfile<MappingUserProfile>();
                cfg.AddProfile<MappingCRProfile>();
                cfg.AddProfile<MappingDetailProfile>();
            });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            }

            await app.RunAsync();
        }
    }
}
