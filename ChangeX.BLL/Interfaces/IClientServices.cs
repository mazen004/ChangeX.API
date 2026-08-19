using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Interfaces
{
    public interface IClientServices
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client> GetByID(Guid ID);
        Task<Client> Create(Client client);
        Task<Client> Update(Client client);
        Task Delete(Guid ID);
    }
}
