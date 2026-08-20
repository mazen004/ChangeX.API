using ChangeX.BLL.DTOs;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;
using ChangeX.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Project
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectAdminController : ControllerBase
    {
        private readonly IProjectService _projectService ;

        public ProjectAdminController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Project
        [HttpGet]
        public IActionResult GetProjects()
        {
            try
            {
                var projects = _projectService.GetProjectsAsync();

                return Ok(new
                {
                    message = "Get all projects",
                    data = projects
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Project/{id}
        [HttpGet("{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Project
        [HttpPost]
        public IActionResult CreateProject(ProjectDto projectDto)
        {
            try
            {
                var project = _projectService.CreateProjectAsync(projectDto);

                return Ok(new
                {
                    message = "Project created successfully",
                    data = project
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Project/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProject(Guid id, ProjectDto projectDto)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(Guid id)
        {
            try
            {
                var result = await  _projectService.DeleteProjectAsync(id);


                if (result)
                {
                    return NotFound(new
                    {
                        message = "Project not found"
                    });
                }

                return Ok(new
                {
                    message = "project not found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}