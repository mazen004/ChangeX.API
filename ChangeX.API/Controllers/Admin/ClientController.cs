using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.DTOs;
using AutoMapper;
using ChangeX.DAL.Entities;


namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientServices clientSercivies;
        private readonly IMapper _mapper;

        public ClientController(IClientServices clientSercivies , IMapper mapper)
        {
            this.clientSercivies = clientSercivies;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var result = await clientSercivies.GetAll();
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var result = await clientSercivies.GetByID(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromForm] ClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            var result = await clientSercivies.Create(client);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = result.Message, data = result.Data });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientDto clientDto)
        {
            var getResult = await clientSercivies.GetByID(id);
            if (!getResult.Success)
                return StatusCode(getResult.StatusCode, new { message = getResult.Message });

            _mapper.Map(clientDto, getResult.Data);
            var result = await clientSercivies.Update(getResult.Data!);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var getResult = await clientSercivies.GetByID(id);
            if (!getResult.Success)
                return StatusCode(getResult.StatusCode, new { message = getResult.Message });

            var result = await clientSercivies.Delete(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
