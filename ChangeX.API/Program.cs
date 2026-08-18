using ChangeX.BLL.Services;
using ChangeX.DAL.Database;
using ChangeX.BLL.Interfaces;
using ChangeX.BLL.SeedData;
using ChangeX.BLL.StatusMachine;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<ICRService, CRService>();

            builder.Services.AddAutoMapper(_ => { }, typeof(Program));

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

            await using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ApplicationContext>();
                var insertedStatuses = await StatusWorkflowSeeder.SeedAsync(dbContext);

                if (insertedStatuses > 0)
                {
                    app.Logger.LogInformation(
                        "Inserted {StatusCount} missing CR workflow statuses",
                        insertedStatuses);
                }

                if (app.Environment.IsDevelopment())
                {
                    var insertedSampleRecords =
                        await DevelopmentSampleDataSeeder.SeedAsync(dbContext);

                    app.Logger.LogInformation(
                        "Development sample data is ready. ClientId: {ClientId}; ProjectId: {ProjectId}; Inserted: {InsertedCount}",
                        DevelopmentSampleDataSeeder.ClientId,
                        DevelopmentSampleDataSeeder.ProjectId,
                        insertedSampleRecords);
                }
            }

            await app.RunAsync();
        }
    }
}
