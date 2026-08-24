using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CRController(ICurrentUserService currentUser, ICRServices crService, IMapper mapper) : ControllerBase 
    {
        [HttpGet("GetAllCRs")]
        public async Task<IActionResult> GetAllCRs([FromQuery] Guid? projectID, [FromQuery] Guid? ClientID, [FromQuery] Guid? statusId, [FromQuery] string? name)
        {
            Expression<Func<CR, bool>>? predicate = null;

            if (currentUser.Role != "Admin" && currentUser.ClientId != ClientID)
                return Forbid();
            
            if (projectID.HasValue || statusId.HasValue || !string.IsNullOrWhiteSpace(name) || ClientID.HasValue)
            {
                predicate = cr =>
                    (!projectID.HasValue || cr.ProjectID == projectID.Value) &&
                    (!statusId.HasValue || cr.CurrentStatusID == statusId.Value) &&
                    (string.IsNullOrWhiteSpace(name) || cr.Name.Contains(name)) &&
                    (!ClientID.HasValue || cr.Project.ClientID == ClientID);
            }

            var result = await crService.GetAll(predicate);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            var data = mapper.Map<IEnumerable<CRResponseDto>>(result.Data);

            return Ok(new
            {
                message = result.Message,
                data
            });
        }

        [HttpGet("GetCR/{ID:Guid}")]
        public async Task<IActionResult> GetCRById(Guid ID)
        {
            var result = await crService.GetByID(ID);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            var data = mapper.Map<CRResponseDto>(result.Data);

            return Ok(new
            {
                message = result.Message,
                data
            });
        }
         
        [HttpPost("AddCR")]
        public async Task<IActionResult> CreateCR([FromBody] CreateCRDto crDto)
        {
            var cr = mapper.Map<CR>(crDto);
            var result = await crService.Create(cr);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpPut("UpdateCR"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCR([FromQuery] Guid ID, [FromBody] EstimateCRDto crDto)
        {
            var getResult = await crService.GetByID(ID);
            if (!getResult.Success)
                return StatusCode(getResult.StatusCode, new { message = getResult.Message });

            mapper.Map(crDto, getResult.Data);
            var result = await crService.Update(getResult.Data!);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpDelete("DeleteCR"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCR([FromQuery] Guid ID)
        {
            var result = await crService.Delete(ID);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new
            {
                message = result.Message
            });
        }

        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] Guid ID, [FromQuery] Guid CRID)
        {
            var crResult = await crService.GetByID(CRID);
            if (!crResult.Success)
                return StatusCode(crResult.StatusCode, new { message = crResult.Message });

            var result = await crService.ChangeStatus(ID, crResult.Data!);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }
    }
}
