using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Interfaces
{
    public interface IClientServices
    {
        Task<ServiceResponse<IEnumerable<Client>>> GetAll();
        Task<ServiceResponse<Client>> GetByID(Guid ID);
        Task<ServiceResponse<Client>> Create(Client client);
        Task<ServiceResponse<Client>> Update(Client client);
        Task<ServiceResponse<bool>> Delete(Guid ID);
    }
}
