using ChangeX.DAL.Entities;
using System.Linq.Expressions;

namespace ChangeX.BLL.Interfaces
{
    public interface ICRServices
    {
        Task<IEnumerable<CR>> GetAll(Expression<Func<CR, bool>>? predicate);
        Task<CR?> GetByID(Guid ID);
        Task<CR> Create(CR cr);
        Task<CR> Update(CR cr);
        Task Delete(Guid ID);
    }
}


