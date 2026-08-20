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

            var crs = await _crService.GetAll(predicate);
            var data = _mapper.Map<IEnumerable<CRResponseDto>>(crs);

            return Ok(new
            {
                message = "Get all CRs",
                data
            });
        }

        // GET: api/CR/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCRById(Guid id)
        {
            var cr = await _crService.GetByID(id);

            if (cr == null)
            {
                return NotFound(new
                {
                    message = "CR not found"
                });
            }

            var data = _mapper.Map<CRResponseDto>(cr);

            return Ok(new
            {
                message = "CR found",
                data
            });
        }

        // POST: api/CR
        [HttpPost]
        public async Task<IActionResult> CreateCR([FromBody] CRDto crDto)
        {
            var cr = _mapper.Map<CR>(crDto);
            var created = await _crService.Create(cr);

            return Ok(new
            {
                message = "CR created successfully",
                data = created
            });
        }

        // PUT: api/CR/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCR(Guid id, [FromBody] CRDto crDto)
        {
            var cr = await _crService.GetByID(id);
            if (cr == null)
            {
                return NotFound(new
                {
                    message = "CR not found"
                });
            }

            _mapper.Map(crDto, cr);
            var updated = await _crService.Update(cr);

            return Ok(new
            {
                message = "CR updated successfully",
                data = updated
            });
        }

        // DELETE: api/CR/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCR(Guid id)
        {
            await _crService.Delete(id);

            return Ok(new
            {
                message = "CR deleted successfully"
            });
        }
        
        
    }
}



