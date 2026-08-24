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
        public async Task<IEnumerable<Project>> GetProjectsAsync(Guid ClientId)
        {
            return await dbcontext.Projects.Where(x=>x.ClientID == ClientId).ToListAsync();
        }


        public async Task<IEnumerable<Project>> GetProjectsAsync(Expression<Func<Project, bool>>? predicate)
        {
            var query = dbcontext.Projects.AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(Guid id)
        {
            return await dbcontext.Projects.FindAsync(id);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            var createProject = new Project()
            {
                ID = Guid.NewGuid(),
                Name = project.Name,
                Description = project.Description,
                Scope = project.Scope,
                ClientID = project.ClientID,
                State = project.State
            };

            dbcontext.Projects.Add(createProject);
            await dbcontext.SaveChangesAsync();

            return createProject;
        }

        public async Task<Project?> UpdateProjectAsync(Guid id, Project project)
        {
            var UpdateProject = await dbcontext.Projects.FindAsync(id);

            if (UpdateProject == null)
            {
                return null;
            }

            UpdateProject.Name = project.Name;
            UpdateProject.Description = project.Description;
            UpdateProject.Scope = project.Scope;
            UpdateProject.ClientID = project.ClientID;
            UpdateProject.State = project.State;

            await dbcontext.SaveChangesAsync();

            return UpdateProject;
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