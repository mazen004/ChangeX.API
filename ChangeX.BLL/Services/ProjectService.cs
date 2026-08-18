using ChangeX.BLL.DTOs;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext dbcontext;

        public ProjectService(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        // Get all projects
        public List<Project> GetProjects()
        {
            return dbcontext.Projects.ToList();
        }

        // Get project by ID
        public Project? GetProjectById(Guid id)
        {
            return dbcontext.Projects.Find(id);
        }

        // Create project
        public Project CreateProject(ProjectDto projectDto)
        {
            var project = new Project()
            {
                ID = Guid.NewGuid(),
                Name = projectDto.Name,
                Description = projectDto.Description,
                Scope = projectDto.Scope,
                ClientID = projectDto.ClientID,
                State = projectDto.State
            };

            dbcontext.Projects.Add(project);
            dbcontext.SaveChanges();

            return project;
        }

        // Update project
        public Project? UpdateProject(Guid id, ProjectDto projectDto)
        {
            var project = dbcontext.Projects.Find(id);

            if (project == null)
            {
                return null;
            }

            project.Name = projectDto.Name;
            project.Description = projectDto.Description;
            project.Scope = projectDto.Scope;
            project.ClientID = projectDto.ClientID;
            project.State = projectDto.State;

            dbcontext.SaveChanges();

            return project;
        }

        // Delete project
        public bool DeleteProject(Guid id)
        {
            var project = dbcontext.Projects.Find(id);

            if (project == null)
            {
                return false;
            }

            dbcontext.Projects.Remove(project);
            dbcontext.SaveChanges();

            return true;
        }

        // ================= Async versions =================

        public async Task<List<Project>> GetProjectsAsync(Guid ClientId)
        {
            return await dbcontext.Projects.Where(x=>x.ClientID == ClientId).ToListAsync();
        }


        public async Task<List<Project>> GetProjectsAsync()
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