using ChangeX.DAL.Database;
using ChangeX.BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.DTOs.Users;

namespace ChangeX.API.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectClientController : ControllerBase
    {
        private readonly ApplicationContext dbcontext;

        public ProjectClientController(ApplicationContext _dbcontext)
        {
            this.dbcontext = _dbcontext;
        }

        // GET: api/Project
        [HttpGet]
        public IActionResult GetProjects()
        {
            return Ok(new
            {
                message = "Get all projects",
                data = dbcontext.Projects.ToList()
            });
        }

        // GET: api/Project/{id}
        [HttpGet("{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            var project = dbcontext.Projects.Find(id);

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

            dbcontext.Projects.Add(project);
            dbcontext.SaveChanges();

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
            var project = dbcontext.Projects.Find(id);

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

            dbcontext.SaveChanges();

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
            var project = dbcontext.Projects.Find(id);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Project not found"
                });
            }

            dbcontext.Projects.Remove(project);
            dbcontext.SaveChanges();

            return Ok(new
            {
                message = "Project deleted successfully"
            });
        }
    }
}