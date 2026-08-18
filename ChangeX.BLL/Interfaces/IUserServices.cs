using ChangeX.DAL.Entities;

namespace ChangeX.BLL.Services
{
    public interface IUserServices
    {
        public Task<IEnumerable<User>> GetAll();
        public Task<IEnumerable<User>> GetAll(Guid ClientID);
        public Task AddUser(User User);
    }
}