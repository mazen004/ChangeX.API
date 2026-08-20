using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService statusService;

        public StatusController(IStatusService statusService)
        {
            this.statusService = statusService;
        }

        [HttpGet("cr/{crId:guid}")]
        public async Task<IActionResult> GetCurrentStatus(Guid crId)
        {
            try
            {
                var status = await statusService.GetCurrentStatus(crId);
                return Ok(new
                {
                    message = "Status found",
                    data = status
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("cr/{crId:guid}/available")]
        public async Task<IActionResult> GetAvailableStatus(Guid crId)
        {
            try
            {
                var availableStatuses = await statusService.GetAvailableStatus(crId);
                return Ok(new
                {
                    message = "Available statuses found",
                    data = availableStatuses
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
