using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;
using System.Linq.Expressions;

namespace ChangeX.BLL.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetProjectsAsync(
            Expression<Func<Project, bool>>? predicate);

        Task<IEnumerable<Project>> GetProjectsAsync(Guid clientId);

        Task<Project?> GetProjectByIdAsync(Guid id);

        Task<Project> CreateProjectAsync(Project project);

        Task<Project?> UpdateProjectAsync(Guid id, Project project);

        Task<bool> DeleteProjectAsync(Guid id);
    }
}