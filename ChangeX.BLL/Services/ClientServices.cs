using ChangeX.BLL.DTOs;
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

        public async Task<ServiceResponse<IEnumerable<Client>>> GetAll()
        {
            var clients = await dbcontext.Clients
                .AsNoTracking()
                .Include(c => c.DefaultContact)
                .ToListAsync();
            return ServiceResponse<IEnumerable<Client>>.Ok(clients, "Get all clients");
        }
        public async Task<ServiceResponse<Client>> GetByID(Guid ID)
        {
            var client = await dbcontext.Clients
                .AsNoTracking()
                .Where(c => c.ID == ID)
                .Include(c => c.DefaultContact)
                .FirstOrDefaultAsync();
            if (client == null)
                return ServiceResponse<Client>.Fail("Client not found.", 404);
            return ServiceResponse<Client>.Ok(client, "Client found");
        }
        public async Task<ServiceResponse<Client>> Create(Client client)
        {
            await dbcontext.Clients.AddAsync(client);
            await dbcontext.SaveChangesAsync();
            return ServiceResponse<Client>.Ok(client, "Client created successfully");
        }
        public async Task<ServiceResponse<Client>> Update(Client client)
        {
            var existingClient = await dbcontext.Clients
                .Where(c => c.ID == client.ID)
                .FirstOrDefaultAsync();
            if (existingClient == null)
                return ServiceResponse<Client>.Fail("Client not found.", 404);
            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            existingClient.Description = client.Description;
            existingClient.Address = client.Address;
            existingClient.ContactInfo = client.ContactInfo;
            await dbcontext.SaveChangesAsync();
            return ServiceResponse<Client>.Ok(existingClient, "Client updated successfully");
        }
        public async Task<ServiceResponse<bool>> Delete(Guid ID)
        {
            var existingClient = await dbcontext.Clients
                .Where(c => c.ID == ID)
                .FirstOrDefaultAsync();
            if (existingClient == null)
                return ServiceResponse<bool>.Fail("Client not found.", 404);
            dbcontext.Clients.Remove(existingClient);
            await dbcontext.SaveChangesAsync();
            return ServiceResponse<bool>.Ok(true, "Client deleted successfully");
        }
    }
}
