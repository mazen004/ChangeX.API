using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRAdminController : ControllerBase
    {
        private readonly ICRService _crService;

        public CRAdminController(ICRService crService)
        {
            _crService = crService;
        }

        [HttpPut("{crId:guid}/feedback")]
        public Task<IActionResult> SubmitFeedback(
            Guid crId,
            [FromBody] AdminFeedbackDto dto)
        {
            return ExecuteAsync(
                () => _crService.SubmitAdminFeedbackAsync(crId, dto),
                "Admin feedback submitted");
        }

        [HttpPut("{crId:guid}/stage")]
        public Task<IActionResult> ChangeStage(
            Guid crId,
            [FromBody] ChangeStageDto dto)
        {
            return ExecuteAsync(
                () => _crService.ChangeStageAsync(crId, dto),
                "CR stage changed");
        }

        private async Task<IActionResult> ExecuteAsync(
            Func<Task<CRWorkflowResponseDto>> action,
            string message)
        {
            try
            {
                var result = await action();
                return Ok(new { message, data = result });
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
