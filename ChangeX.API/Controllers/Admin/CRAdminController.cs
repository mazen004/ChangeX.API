using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRAdminController : ControllerBase
    {
        private readonly ICRService _crService;
        public CRAdminController(ICRService crService) => _crService = crService;
        [HttpPost("{crId}/accept")]
        public async Task<IActionResult> AcceptCR(Guid crId)
            => await ChangeStatus(crId, "Accepted", "Admin");
        [HttpPost("{crId}/reject")]
        public async Task<IActionResult> RejectCR(Guid crId)
            => await ChangeStatus(crId, "Rejected", "Admin");
        [HttpPost("{crId}/request-clarification")]
        public async Task<IActionResult> RequestClarification(Guid crId)
            => await ChangeStatus(crId, "ClarificationRequested", "Admin");
        [HttpPost("{crId}/estimate")]
        public async Task<IActionResult> EstimateCR(Guid crId, EstimateCRDto dto)
        {
            try
            {
                var cr = await _crService.EstimateCRAsync(crId, dto);
                return Ok(new { message = "Estimate submitted", data = cr });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }
        private async Task<IActionResult> ChangeStatus(Guid crId, string status, string role)
        {
            try
            {
                var cr = await _crService.ChangeStatusAsync(crId, status, role);
                return Ok(new { message = $"CR {status.ToLower()}", data = cr });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        }
    }
}