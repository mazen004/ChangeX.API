using ChangeX.BLL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestCRController : ControllerBase
    {

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
    }
    }

