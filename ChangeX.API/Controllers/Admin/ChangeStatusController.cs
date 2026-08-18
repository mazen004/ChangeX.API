using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeStatusController : ControllerBase
    {
        private readonly ICRService _crService;

        public ChangeStatusController(ICRService crService)
        {
            _crService = crService;
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusDto dto)
        {
            try
            {
                var cr = await _crService.ChangeStatusAsync(
                    dto.CRID,
                    dto.TargetStatus,
                    dto.ActorRole);

                return Ok(new { message = "Status changed", data = cr });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
