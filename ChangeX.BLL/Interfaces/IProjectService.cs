using ChangeX.BLL.DTOs;
using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Entities;

namespace ChangeX.BLL.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsAsync();
        Task<List<Project>> GetProjectsAsync(Guid ClientId);
        Task<Project?> GetProjectByIdAsync(Guid id);

        Task<Project> CreateProjectAsync(ProjectDto dto);

        Task<Project?> UpdateProjectAsync(Guid id, ProjectDto dto);

        Task<bool> DeleteProjectAsync(Guid id);
    }
}