using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.DTOs;


namespace ChangeX.API.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationContext dbcontext;

        public ClientController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            // Logic to retrieve clients from the database
            return Ok(new { message = "Get all clients", data= dbcontext.Clients.ToList()});
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(Guid id)
        {
            // Logic to retrieve a specific client from the database
            var client = dbcontext.Clients.Find(id);
            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }
            return Ok(new { message = "Client found", data = client });
        }

            [HttpPost]
        public IActionResult CreateClient(ClientDto clientDto)
        {
            // Logic to create a new client in the database
<<<<<<< HEAD
            var client = new  ()
=======
            var client = new DAL.Entities.Client()
>>>>>>> 6bd110a6e1f73edd68658a2eb287278cd1bd02ed
            {
                Name = clientDto.Name,
                Email = clientDto.Email,
                Description = clientDto.Description,
                Address = clientDto.Address,
                ContactInfo = clientDto.ContactInfo
            };

            dbcontext.Clients.Add(client);
            dbcontext.SaveChanges();

            return Ok(new { message = "Client created successfully", data = client });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(Guid id, ClientDto clientDto)
        {
            // Logic to update an existing client in the database
            var client = dbcontext.Clients.Find(id);
            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            client.Name = clientDto.Name;
            client.Email = clientDto.Email;
            client.Description = clientDto.Description;
            client.Address = clientDto.Address;
            client.ContactInfo = clientDto.ContactInfo;

            dbcontext.SaveChanges();
            return Ok(new { message = "Client updated successfully", data = client });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(Guid id)
        {
            // Logic to delete a client from the database
            var client = dbcontext.Clients.Find(id);
            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            dbcontext.Clients.Remove(client);
            dbcontext.SaveChanges();
            return Ok(new { message = "Client deleted successfully" });
        }
    }
}
