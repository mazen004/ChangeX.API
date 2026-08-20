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
    public class DetailController : ControllerBase
    {
        private readonly IDetailServices detailServices;
        private readonly IMapper mapper;

        public DetailController(IDetailServices detailServices, IMapper mapper)
        {
            this.detailServices = detailServices;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(
            [FromQuery] Guid? crId,
            [FromQuery] string? state)
        {
            Expression<Func<Detail, bool>>? predicate = null;

            if (crId.HasValue || !string.IsNullOrWhiteSpace(state))
            {
                predicate = detail =>
                    (!crId.HasValue || detail.CRID == crId.Value) &&
                    (string.IsNullOrWhiteSpace(state) || detail.State == state);
            }

            var result = await detailServices.GetAll(predicate);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetailById(Guid id)
        {
            var result = await detailServices.GetByID(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDetail([FromBody] DetailDto detailDto)
        {
            var detail = mapper.Map<Detail>(detailDto);
            var result = await detailServices.Create(detail);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = result.Message, data = result.Data });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDetail(Guid id, [FromBody] DetailDto detailDto)
        {
            var getResult = await detailServices.GetByID(id);
            if (!getResult.Success)
            {
                return StatusCode(getResult.StatusCode, new { message = getResult.Message });
            }

            mapper.Map(detailDto, getResult.Data);
            var result = await detailServices.Update(getResult.Data!);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            var result = await detailServices.Delete(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
