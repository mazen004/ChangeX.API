using ChangeX.DAL.Database;
using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.DTOs;
using Microsoft.EntityFrameworkCore;
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
            var clients = await clientSercivies.GetAll();

            return Ok(new { message = "Get all clients", data = clients });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var client = await clientSercivies.GetByID(id);

            if (client is null)
            {
                return NotFound(new { message = "Client not found" });
            }

            return Ok(new { message = "Client found", data = client });
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromForm] ClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            await clientSercivies.Create(client);

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "Client created successfully", data = client });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientDto clientDto)
        {
            var client = await clientSercivies.GetByID(id);
            if (client is null)
            {
                return NotFound(new { message = "Client not found" });
            }
             
            return Ok(new { message = "Client updated successfully", data = client });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var client = await clientSercivies.GetByID(id);
            if (client is null)
            {
                return NotFound(new { message = "Client not found" });
            }

            await clientSercivies.Delete(id);
            return Ok(new { message = "Client deleted successfully" });
        }
    }
}
