using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusController(IStatusService statusService) : ControllerBase
    {

        [HttpGet("CR/{CRID:guid}")]
        public async Task<IActionResult> GetCurrentStatus(Guid CRID)
        {
            try
            {
                var status = await statusService.GetCurrentStatus(CRID);
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

        [HttpGet("AvailableCRStatus/{CRID:guid}")]
        public async Task<IActionResult> GetAvailableStatus(Guid CRID)
        {
            try
            {
                var availableStatuses = await statusService.GetAvailableStatus(CRID);
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
