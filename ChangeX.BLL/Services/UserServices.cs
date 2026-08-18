using ChangeX.DAL.Entities;
using ChangeX.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationContext _dbContext;

        public UserServices(ApplicationContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<User>> GetAll(Guid ClientID, bool search = false)
        {
            return await _dbContext.Users
                        .Where(u => u.ClientID == ClientID && search)
                        .Include(u => u.Client)
                        .ToListAsync();
        }
    }
}