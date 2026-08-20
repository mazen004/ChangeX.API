using ChangeX.DAL.Entities;
using Microsoft.Identity.Client;

namespace ChangeX.BLL.Services
{
    public interface IUserServices
    {
        public Task<IEnumerable<User>> GetAll(string? search = null);
        public Task<IEnumerable<User>> GetAll(Guid ClientID, string? search = null);
        public Task<User> GetByID(Guid ID);
        public Task AddUser(User User);
        public Task<bool> CouldBeDefault(Guid ClientID);
        public Task<bool> IsUserFound(string Email);
        public Task<User> UpdateUser(User User);
        public Task DeleteUser(User User);
    }
}