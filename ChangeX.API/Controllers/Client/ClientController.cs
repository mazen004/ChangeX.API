using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;


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

        [HttpPost]
        public IActionResult CreateClient(ClientDto clientDto)
        {
            // Logic to create a new client in the database
            var client = new  ()
            {
                Name = clientDto.Name,
                Email = clientDto.Email,
                ContactInfo = clientDto.ContactInfo
            };
            dbcontext.Clients.Add(client);
            dbcontext.SaveChanges();
            return Ok(new { message = "Client created successfully", data = client });
        }
    }
}
