using ChangeX.DAL.Entities;
using Microsoft.Identity.Client;

namespace ChangeX.BLL.Services
{
    public interface IUserServices
    {
        public Task<IEnumerable<User>> GetAll();
        public Task<IEnumerable<User>> GetAll(Guid ClientID);
        public Task AddUser(User User);
        public Task<bool> CouldBeDefault(Guid ClientID);
        public Task<bool> IsClientVailed(Guid ClientID);
    }
}