using ChangeX.BLL.DTOs;
using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ProjectAdminController : ControllerBase
    {
        private readonly ApplicationContext _dbcontext;

        public ProjectAdminController(ApplicationContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        // GET: api/Project
        [HttpGet]
        public IActionResult GetProjects()
        {
            return Ok(new
            {
                message = "Get all projects",
                data = _dbcontext.Projects.ToList()
            });
        }

        // GET: api/Project/{id}
        [HttpGet("{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            var project = _dbcontext.Projects.Find(id);

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
            var project = new DAL.Entities.Project()
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                Scope = projectDto.Scope,
                ClientID = projectDto.ClientID,
                State = projectDto.State
            };

            _dbcontext.Projects.Add(project);
            _dbcontext.SaveChanges();

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
            var project = _dbcontext.Projects.Find(id);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Project not found"
                });
            }

            project.Name = projectDto.Name;
            project.Description = projectDto.Description;
            project.Scope = projectDto.Scope;
            project.ClientID = projectDto.ClientID;
            project.State = projectDto.State;

            _dbcontext.SaveChanges();

            return Ok(new
            {
                message = "Project updated successfully",
                data = project
            });
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(Guid id)
        {
            var project = _dbcontext.Projects.Find(id);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Project not found"
                });
            }

            _dbcontext.Projects.Remove(project);
            _dbcontext.SaveChanges();

            return Ok(new
            {
                message = "Project deleted successfully"
            });
        }
    }
}