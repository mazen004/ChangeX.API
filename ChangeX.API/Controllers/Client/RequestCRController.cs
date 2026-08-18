using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestCRController : ControllerBase
    {
        private readonly ICRService _crService;

        public RequestCRController(ICRService crService)
        {
            _crService = crService;
        }

        [HttpGet("{crId:guid}")]
        public Task<IActionResult> GetWorkflow(Guid crId)
        {
            return ExecuteAsync(
                () => _crService.GetWorkflowAsync(crId),
                "CR workflow retrieved");
        }

        [HttpPost]
        public Task<IActionResult> RequestCR(
            [FromQuery] Guid clientId,
            [FromBody] RequestCRDto dto)
        {
            return ExecuteAsync(
                () => _crService.RequestCRAsync(dto, clientId),
                "CR submitted successfully",
                StatusCodes.Status201Created);
        }

        [HttpPut("{crId:guid}/clarify")]
        public Task<IActionResult> SubmitClarification(
            Guid crId,
            [FromBody] DetailDto dto)
        {
            return ExecuteAsync(
                () => _crService.SubmitClarificationAsync(crId, dto),
                "Clarification submitted");
        }

        [HttpPut("{crId:guid}/estimate-decision")]
        public Task<IActionResult> SubmitEstimateDecision(
            Guid crId,
            [FromBody] EstimateDecisionDto dto)
        {
            return ExecuteAsync(
                () => _crService.SubmitEstimateDecisionAsync(crId, dto),
                "Estimate decision submitted");
        }

        [HttpPut("{crId:guid}/approval")]
        public Task<IActionResult> SubmitClientApproval(
            Guid crId,
            [FromBody] ClientApprovalDto dto)
        {
            return ExecuteAsync(
                () => _crService.SubmitClientApprovalAsync(crId, dto),
                "Client approval submitted");
        }

        private async Task<IActionResult> ExecuteAsync(
            Func<Task<CRWorkflowResponseDto>> action,
            string message,
            int successStatus = StatusCodes.Status200OK)
        {
            try
            {
                var result = await action();
                return StatusCode(successStatus, new { message, data = result });
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
