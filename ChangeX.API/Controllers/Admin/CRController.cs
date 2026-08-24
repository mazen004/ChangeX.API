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

        // GET: api/CR
        [HttpGet]
        public async Task<IActionResult> GetAllCRs([FromQuery] Guid? projectId, [FromQuery] Guid? ClientID, [FromQuery] Guid? statusId, [FromQuery] string? name)
        {
            Expression<Func<CR, bool>>? predicate = null;

            if (currentUser.Role != "Admin" && currentUser.ClientId != ClientID)
                return Unauthorized();
            
            if (projectId.HasValue || statusId.HasValue || !string.IsNullOrWhiteSpace(name) || ClientID.HasValue)
            {
                predicate = cr =>
                    (!projectId.HasValue || cr.ProjectID == projectId.Value) &&
                    (!statusId.HasValue || cr.CurrentStatusID == statusId.Value) &&
                    (string.IsNullOrWhiteSpace(name) || cr.Name.Contains(name) &&
                    (!ClientID.HasValue || cr.Project.ClientID == ClientID));
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

        // GET: api/CR/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCRById(Guid id)
        {
            var result = await crService.GetByID(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            var data = mapper.Map<CRResponseDto>(result.Data);

            return Ok(new
            {
                message = result.Message,
                data
            });
        }

        // POST: api/CR
        [HttpPost]
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

        // PUT: api/CR/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCR(Guid id, [FromBody] EstimateCRDto crDto)
        {
            var getResult = await crService.GetByID(id);
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

        // DELETE: api/CR/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCR(Guid id)
        {
            var result = await crService.Delete(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new
            {
                message = result.Message
            });
        }

        [HttpPut("change_status/{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id, Guid CRID)
        {
            var crResult = await crService.GetByID(CRID);
            if (!crResult.Success)
                return StatusCode(crResult.StatusCode, new { message = crResult.Message });

            var result = await crService.ChangeStatus(id, crResult.Data!);
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
