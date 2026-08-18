using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using ChangeX.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.SeedData
{
    public static class DevelopmentSampleDataSeeder
    {
        public static readonly Guid ClientId =
            Guid.Parse("a1000000-0000-0000-0000-000000000001");

        public static readonly Guid ProjectId =
            Guid.Parse("a2000000-0000-0000-0000-000000000001");

        public static async Task<int> SeedAsync(ApplicationContext dbContext)
        {
            var insertedRecords = 0;

            var clientExists = await dbContext.Clients
                .AsNoTracking()
                .AnyAsync(client => client.ID == ClientId);

            if (!clientExists)
            {
                dbContext.Clients.Add(new Client
                {
                    ID = ClientId,
                    Name = "Abdelrhman Demo Client",
                    Email = "abdelrhman.demo@changex.local",
                    Description = "Development client for testing the CR workflow",
                    Address = "Cairo, Egypt",
                    ContactInfo = "+20 100 000 0000"
                });

                insertedRecords++;
            }

            var projectExists = await dbContext.Projects
                .AsNoTracking()
                .AnyAsync(project => project.ID == ProjectId);

            if (!projectExists)
            {
                dbContext.Projects.Add(new Project
                {
                    ID = ProjectId,
                    Name = "ChangeX Demo Project",
                    Description = "Development project for testing change requests",
                    Scope = "CR workflow and invoice flow",
                    ClientID = ClientId,
                    State = ProjectState.Active
                });

                insertedRecords++;
            }

            if (insertedRecords > 0)
            {
                await dbContext.SaveChangesAsync();
            }

            return insertedRecords;
        }
    }
}
