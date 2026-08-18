using ChangeX.BLL.DTOs;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;
using ChangeX.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Project
        [HttpGet]
        public IActionResult GetProjects()
        {
            var projects = _projectService.GetProjectsAsync();

            return Ok(new
            {
                message = "Get all projects",
                data = projects
            });
        }

        // GET: api/Project/{id}
        [HttpGet("{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            var project = _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Project not found"
                });
            }

            return Ok(new
            {
                message = "Project found",
                data = project
            });
        }

        // POST: api/Project
        [HttpPost]
        public IActionResult CreateProject(ProjectDto projectDto)
        {
            var project = _projectService.CreateProjectAsync(projectDto);

            return Ok(new
            {
                message = "Project created successfully",
                data = project
            });
        }

        // PUT: api/Project/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProject(Guid id, ProjectDto projectDto)
        {
            var project = _projectService.UpdateProjectAsync(id, projectDto);
           
           
            if (project == null)
            {
                return NotFound(new
                {
                    message = "Project not found"
                });
            }

            return Ok(new
            {
                message = "Project updated successfully",
                data = project
            });
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(Guid id)
        {
            var result = await _projectService.DeleteProjectAsync(id);


            if (!result)
            {
                return NotFound(new
                {
                    message = "Project not found"
                });
            }

            return Ok(new
            {
                message = "Project deleted successfully"
            });
        }
    }
}