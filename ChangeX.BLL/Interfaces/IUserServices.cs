using ChangeX.DAL.Entities;

namespace ChangeX.BLL.Services
{
    public interface IUserServices
    {
        public Task<List<User>> GetAll(Guid ClientID, Boolean search = false);
    }
}