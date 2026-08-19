using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Services
{
    public class ClientServices : IClientServices
    {
        private readonly ApplicationContext dbcontext;

        public ClientServices(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await dbcontext.Clients
                .ToListAsync();
        }
        public async Task<Client> GetByID(Guid ID)
        {
            var client = await dbcontext.Clients
                .Where(c => c.ID == ID)
                .FirstOrDefaultAsync();
            if (client == null)
                throw new Exception($"Client not found.");
            return client;
        }
        public async Task<Client> Create(Client client)
        {
            await dbcontext.Clients.AddAsync(client);
            await dbcontext.SaveChangesAsync();
            return client;
        }
        public async Task<Client> Update(Client client)
        {
            var existingClient = await dbcontext.Clients
                .Where(c => c.ID == client.ID)
                .FirstOrDefaultAsync();
            if (existingClient == null)
                throw new Exception($"Client not found.");
            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            existingClient.Description = client.Description;
            existingClient.Address = client.Address;
            existingClient.ContactInfo = client.ContactInfo;
            await dbcontext.SaveChangesAsync();
            return existingClient;
        }
        public async Task Delete(Guid ID)
        {
            var existingClient = await dbcontext.Clients
                .Where(c => c.ID == ID)
                .FirstOrDefaultAsync();
            if (existingClient == null)
                throw new Exception($"Client not found.");
            dbcontext.Clients.Remove(existingClient);
            await dbcontext.SaveChangesAsync();
        }
    }
}
