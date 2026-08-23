using System.Linq.Expressions;
using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;

namespace ChangeX.BLL.Interfaces
{
    public interface IUserServices
    {
        public Task<ServiceResponse<IEnumerable<User>>> GetAll(Expression<Func<User, bool>>? predicate);
        public Task<ServiceResponse<IEnumerable<User>>> GetAll(Guid ClientID, Expression<Func<User, bool>>? predicate);
        public Task<ServiceResponse<User>> GetByID(Guid ID);
        public Task<ServiceResponse<User>> AddUser(User User);
        public Task<ServiceResponse<User>> UpdateUser(User User);
        public Task<ServiceResponse<bool>> DeleteUser(User User);
        public Task<ServiceResponse<bool>> IsUserExists(string Email, string PhoneNumber);
        public Task<ServiceResponse<bool>> IsInClient(Guid ClientID, Guid UserID);
    }
}