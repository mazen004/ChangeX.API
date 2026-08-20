using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;
using System.Linq.Expressions;

namespace ChangeX.BLL.Interfaces
{
    public interface IDetailServices
    {
        Task<ServiceResponse<IEnumerable<Detail>>> GetAll(
            Expression<Func<Detail, bool>>? predicate);

        Task<ServiceResponse<Detail>> GetByID(Guid ID);
        Task<ServiceResponse<Detail>> Create(Detail detail);
        Task<ServiceResponse<Detail>> Update(Detail detail);
        Task<ServiceResponse<bool>> Delete(Guid ID);
    }
}
