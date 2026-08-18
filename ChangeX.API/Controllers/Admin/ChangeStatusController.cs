using ChangeX.BLL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    public class ChangeStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("change")]
        public async Task<IActionResult> ChangeStatus(ChangeStatusDto dto)
        {
            try
            {
                var cr = await CRService.ChangeStatusAsync(dto.CRID, dto.TargetStatus, dto.ActorRole);
                return Ok(new { message = "Status changed", data = cr });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        }
    }
}
