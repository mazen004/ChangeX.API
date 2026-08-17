using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ApplicationContext dbcontext;

        public StatusController(ApplicationContext dbcontext) {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("{id:int}")]
       public IActionResult GetAvailableStatus(int id )
        {


            var currentStatus = dbcontext.CRStatues.Find(id);

                if (currentStatus == null)
                {
                    return NotFound(new { message = $"No status found" });
                }
                var availableStatuses = currentStatus.AvailableStatuses
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                return Ok(new
                {
                    id = currentStatus.ID,
                    currentStatus = currentStatus.CurrentStatus,
                    availableStatuses,
                    accessedBy = currentStatus.AccessedBy
                });
            }

      

        }
    }
