using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.DTOs;
using AutoMapper;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Authorization;


namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController(IClientServices clientSercivies, IMapper mapper, ICurrentUserService currentUserService) : ControllerBase
    {

        [HttpGet("GetAllClients"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                var result = await clientSercivies.GetAll();
                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var data = mapper.Map<IEnumerable<ClientResponseDto>>(result.Data);
                return Ok(new { message = result.Message, data });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("GetClient/{ID:guid}")]
        public async Task<IActionResult> GetClientById(Guid ID)
        {
            try
            {
                if (ID != currentUserService.ClientId)
                    return Unauthorized();
                var result = await clientSercivies.GetByID(ID);
                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var data = mapper.Map<ClientResponseDto>(result.Data);
                return Ok(new { message = result.Message, data });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("AddClient"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateClient([FromForm] ClientDto clientDto)
        {
            try
            {
                var client = mapper.Map<Client>(clientDto);
                var result = await clientSercivies.Create(client);
                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var data = mapper.Map<ClientResponseDto>(result.Data);
                return StatusCode(
                    StatusCodes.Status201Created,
                    new { message = result.Message, data });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpPut("UpdateClient/{ID:guid}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClient(Guid ID, [FromBody] ClientDto clientDto)
        {
            try
            {
                var getResult = await clientSercivies.GetByID(ID);
                if (!getResult.Success)
                    return StatusCode(getResult.StatusCode, new { message = getResult.Message });

                mapper.Map(clientDto, getResult.Data);
                var result = await clientSercivies.Update(getResult.Data!);
                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var data = mapper.Map<ClientResponseDto>(result.Data);
                return Ok(new { message = result.Message, data });
            }
            catch(Exception ex) {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpDelete("DeleteClient/{ID:guid}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(Guid ID)
        {
            try
            {
                var getResult = await clientSercivies.GetByID(ID);
                if (!getResult.Success)
                    return StatusCode(getResult.StatusCode, new { message = getResult.Message });

                var result = await clientSercivies.Delete(ID);
                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
    }
}
