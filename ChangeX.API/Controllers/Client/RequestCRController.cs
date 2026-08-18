using ChangeX.BLL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestCRController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> RequestCR([FromQuery] Guid clientId, RequestCRDto dto)
        {
            try
            {
                var cr = await crService.RequestCRAsync(dto, clientId);
                return Ok(new { message = "Change request submitted", data = cr });
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
