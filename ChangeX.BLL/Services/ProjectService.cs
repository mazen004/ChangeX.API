using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChangeX.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext dbcontext;

        public ProjectService(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<List<Project>> GetProjectsAsync(Guid ClientId)
        {
            return await dbcontext.Projects.Where(x=>x.ClientID == ClientId).ToListAsync();
        }


        public async Task<List<Project>> GetProjectsAsync(Expression<Func<Project, bool>>? predicate)
        {
            return await dbcontext.Projects.ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(Guid id)
        {
            return await dbcontext.Projects.FindAsync(id);
        }

        public async Task<Project> CreateProjectAsync(ProjectDto dto)
        {
            var project = new Project()
            {
                ID = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Scope = dto.Scope,
                ClientID = dto.ClientID,
                State = dto.State
            };

            dbcontext.Projects.Add(project);
            await dbcontext.SaveChangesAsync();

            return project;
        }

        public async Task<Project?> UpdateProjectAsync(Guid id, ProjectDto dto)
        {
            var project = await dbcontext.Projects.FindAsync(id);

            if (project == null)
            {
                return null;
            }

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Scope = dto.Scope;
            project.ClientID = dto.ClientID;
            project.State = dto.State;

            await dbcontext.SaveChangesAsync();

            return project;
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await dbcontext.Projects.FindAsync(id);

            if (project == null)
            {
                return false;
            }

            dbcontext.Projects.Remove(project);
            await dbcontext.SaveChangesAsync();

            return true;
        }
    }
}