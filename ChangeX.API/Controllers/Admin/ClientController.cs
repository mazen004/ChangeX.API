using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.DTOs;
using Microsoft.EntityFrameworkCore;


namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;

        public ClientController(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _dbContext.Clients
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { message = "Get all clients", data = clients });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var client = await _dbContext.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(existingClient => existingClient.ID == id);

            if (client is null)
            {
                return NotFound(new { message = "Client not found" });
            }

            return Ok(new { message = "Client found", data = client });
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDto clientDto)
        {
            var client = new DAL.Entities.Client()
            {
                ID = Guid.NewGuid(),
                Name = clientDto.Name,
                Email = clientDto.Email,
                Description = clientDto.Description,
                Address = clientDto.Address,
                ContactInfo = clientDto.ContactInfo
            };

            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync();

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "Client created successfully", data = client });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientDto clientDto)
        {
            var client = await _dbContext.Clients.FindAsync(id);
            if (client is null)
            {
                return NotFound(new { message = "Client not found" });
            }

            client.Name = clientDto.Name;
            client.Email = clientDto.Email;
            client.Description = clientDto.Description;
            client.Address = clientDto.Address;
            client.ContactInfo = clientDto.ContactInfo;

            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Client updated successfully", data = client });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var client = await _dbContext.Clients.FindAsync(id);
            if (client is null)
            {
                return NotFound(new { message = "Client not found" });
            }

            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Client deleted successfully" });
        }
    }
}
