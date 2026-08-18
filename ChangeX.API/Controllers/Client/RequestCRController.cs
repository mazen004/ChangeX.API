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

        [HttpPost]

        public async Task<IActionResult> RequestCR(
            [FromQuery] Guid clientId,
            [FromBody] RequestCRDto dto)
        {

            try
            {
                var cr = await _crService.RequestCRAsync(dto, clientId);
                return StatusCode(
                    StatusCodes.Status201Created,
                    new { message = "CR submitted successfully", data = cr });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{crId:guid}/clarify")]
        public async Task<IActionResult> ClarifyCR(Guid crId, [FromBody] DetailDto dto)
        {
            try
            {
                var detail = await _crService.ClarifyCRAsync(crId, dto);
                return Ok(new { message = "Clarification submitted", data = detail });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }
        [HttpPost("{crId:guid}/accept-estimate")]
        public async Task<IActionResult> AcceptEstimate(Guid crId)
        {
            try
            {
                var invoice = await _crService.AcceptEstimateAsync(crId);
                return Ok(new { message = "Estimate accepted", data = invoice });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        [HttpPost("{crId:guid}/reject-estimate")]
        public async Task<IActionResult> RejectEstimate(Guid crId)
        {
            try
            {
                var cr = await _crService.RejectEstimateAsync(crId);
                return Ok(new { message = "Estimate rejected", data = cr });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

