using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;

        public StatusController(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAvailableStatus(Guid id)
        {
            var currentStatus = await _dbContext.CRStatues
                .AsNoTracking()
                .FirstOrDefaultAsync(status => status.ID == id);

            if (currentStatus is null)
            {
                return NotFound(new { message = "No status found" });
            }

            var availableStatuses = currentStatus.AvailableStatuses
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
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
