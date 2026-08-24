using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ChangeX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController(IMapper mapper, IProjectService projectServices,ICurrentUserService currentUser) : ControllerBase
    {
       
        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects([FromQuery] Guid? ClientID)
        {
            try
            {
                Expression<Func<Project, bool>>? predicate = null;

                if (currentUser.Role != "Admin" && ClientID != currentUser.ClientId)
                {
                    return Forbid();
                }

                if (ClientID.HasValue)
                {
                    predicate = p => p.ClientID == ClientID;
                }
                else if (currentUser.Role != "Admin" && !ClientID.HasValue)
                {
                    predicate = p => p.ClientID == currentUser.ClientId;
                }

                var projects = await projectServices.GetProjectsAsync(predicate);

                if (projects == null)
                {
                    return NotFound(new
                    {
                        message = "No projects found."
                    });
                }

                var data = mapper.Map<IEnumerable<ProjectDto>>(
                    projects
                );

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("GetProject/{ID:Guid}")]
        public async Task<IActionResult> GetProject(Guid ID)
        {
            try
            {
                var project =
                    await projectServices.GetProjectByIdAsync(ID);

                if (project == null)
                {
                    return NotFound(new
                    {
                        message = "Project not found."
                    });
                }

                if (currentUser.Role != "Admin" && project.ClientID != currentUser.ClientId)
                {
                    return Forbid();
                }

                var data = mapper.Map<ProjectDto>(project);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("AddProject"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProject([FromBody] CreateProjectDto projectDto)
        {
            try
            {
                if (projectDto == null)
                {
                    return BadRequest(new
                    {
                        message = "Project data is required."
                    });
                }

                if (currentUser.Role == "UserAdmin" &&
                    projectDto.ClientID != currentUser.ClientId)
                {
                    return Forbid();
                }

                var createdProject = await projectServices.CreateProjectAsync(mapper.Map<Project>(projectDto));

                if (createdProject == null)
                {
                    return BadRequest(new
                    {
                        message = "Failed to create project."
                    });
                }

                var data =
                    mapper.Map<ProjectDto>(createdProject);

                return Ok(new
                {
                    message = "Project created successfully.",
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpPut("UpdateProject"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProject([FromQuery] Guid ID, [FromBody] CreateProjectDto projectDto)
        {
            try
            {
                if (projectDto == null)
                {
                    return BadRequest(new
                    {
                        message = "Project data is required."
                    });
                }

                var existingProject =
                    await projectServices.GetProjectByIdAsync(ID);

                if (existingProject == null)
                {
                    return NotFound(new
                    {
                        message = "Project not found."
                    });
                }

                
                if (currentUser.Role == "UserAdmin" &&
                    existingProject.ClientID != currentUser.ClientId)
                {
                    return Forbid();
                }

               
                if (currentUser.Role == "UserAdmin" &&
                    projectDto.ClientID != currentUser.ClientId)
                {
                    return Forbid();
                }

                var updatedProject =
                    await projectServices.UpdateProjectAsync(
                        ID,
                        mapper.Map<Project>(projectDto));

                if (updatedProject == null)
                {
                    return NotFound(new
                    {
                        message = "Project not found."
                    });
                }

                var data =
                    mapper.Map<ProjectDto>(updatedProject);

                return Ok(new
                {
                    message = "Project updated successfully.",
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpDelete("DeleteProject"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject([FromQuery] Guid ID)
        {
            try
            {
                var project =
                    await projectServices.GetProjectByIdAsync(ID);

                if (project == null)
                {
                    return NotFound(new
                    {
                        message = "Project not found."
                    });
                }

                
                if (currentUser.Role == "UserAdmin" &&
                    project.ClientID != currentUser.ClientId)
                {
                    return Forbid();
                }

                var deleted =
                    await projectServices.DeleteProjectAsync(ID);

                if (!deleted)
                {
                    return BadRequest(new
                    {
                        message = "Failed to delete project."
                    });
                }

                return Ok(new
                {
                    message = "Project deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
    }
}
