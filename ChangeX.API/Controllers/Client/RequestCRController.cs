using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestCRController : ControllerBase
    {
        private readonly ICRService CRService;
        public RequestCRController(ICRService crService) => CRService = crService;

        [HttpPut("{crId}/clarify")]
        public async Task<IActionResult> ClarifyCR(Guid crId, DetailDto dto)
        {
            try
            {
                var detail = await CRService.ClarifyCRAsync(crId, dto);
                return Ok(new { message = "Clarification submitted", data = detail });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }
        [HttpPost("{crId}/accept-estimate")]
        public async Task<IActionResult> AcceptEstimate(Guid crId)
        {
            try
            {
                var cr = await CRService.AcceptEstimateAsync(crId);
                return Ok(new { message = "Estimate accepted", data = cr });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        [HttpPost("{crId}/reject-estimate")]
        public async Task<IActionResult> RejectEstimate(Guid crId)
        {
            try
            {
                var cr = await CRService.RejectEstimateAsync(crId);
                return Ok(new { message = "Estimate rejected", data = cr });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
    }

