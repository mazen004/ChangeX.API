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

        Task<Project> CreateProjectAsync(ProjectDto dto);

        Task<Project?> UpdateProjectAsync(Guid id, ProjectDto dto);

        Task<bool> DeleteProjectAsync(Guid id);
    }
}