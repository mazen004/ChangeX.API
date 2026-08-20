using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRController : ControllerBase
    {
        private readonly ICRServices _crService;
        private readonly IMapper _mapper;

        public CRController(ICRServices crService, IMapper mapper)
        {
            _crService = crService;
            _mapper = mapper;
        }

        // GET: api/CR
        [HttpGet]
        public async Task<IActionResult> GetAllCRs([FromQuery] Guid? projectId, [FromQuery] Guid? statusId, [FromQuery] string? name)
        {
            Expression<Func<CR, bool>>? predicate = null;

            if (projectId.HasValue || statusId.HasValue || !string.IsNullOrWhiteSpace(name))
            {
                predicate = cr =>
                    (!projectId.HasValue || cr.ProjectID == projectId.Value) &&
                    (!statusId.HasValue || cr.CurrentStatusID == statusId.Value) &&
                    (string.IsNullOrWhiteSpace(name) || cr.Name.Contains(name));
            }

            var result = await _crService.GetAll(predicate);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            var data = _mapper.Map<IEnumerable<CRResponseDto>>(result.Data);

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
            var result = await _crService.GetByID(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            var data = _mapper.Map<CRResponseDto>(result.Data);

            return Ok(new
            {
                message = result.Message,
                data
            });
        }

        // POST: api/CR
        [HttpPost]
        public async Task<IActionResult> CreateCR([FromBody] CRDto crDto)
        {
            var cr = _mapper.Map<CR>(crDto);
            var result = await _crService.Create(cr);
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
        public async Task<IActionResult> UpdateCR(Guid id, [FromBody] CRDto crDto)
        {
            var getResult = await _crService.GetByID(id);
            if (!getResult.Success)
                return StatusCode(getResult.StatusCode, new { message = getResult.Message });

            _mapper.Map(crDto, getResult.Data);
            var result = await _crService.Update(getResult.Data!);
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
            var result = await _crService.Delete(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new
            {
                message = result.Message
            });
        }

        [HttpPut("change_status/{id}")]
        public async Task<IActionResult> ChangeStatus([FromBody] Guid id, Guid CRID)
        {
            var crResult = await _crService.GetByID(CRID);
            if (!crResult.Success)
                return StatusCode(crResult.StatusCode, new { message = crResult.Message });

            var result = await _crService.ChangeStatus(id, crResult.Data!);
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
