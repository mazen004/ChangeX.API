using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;
using System.Linq.Expressions;

namespace ChangeX.BLL.Interfaces
{
    public interface ICRServices
    {
        Task<ServiceResponse<IEnumerable<CR>>> GetAll(Expression<Func<CR, bool>>? predicate);
        Task<ServiceResponse<CR>> GetByID(Guid ID);
        Task<ServiceResponse<CR>> Create(CR cr);
        Task<ServiceResponse<CR>> Update(CR cr);
        Task<ServiceResponse<bool>> Delete(Guid ID);
        Task<ServiceResponse<CR>> ChangeStatus(Guid TargetStatusID, CR cr);
        
           
    }
}


