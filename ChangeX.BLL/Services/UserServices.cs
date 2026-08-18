using ChangeX.DAL.Entities;
using ChangeX.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationContext DbContext;

        public UserServices(ApplicationContext DbContext)
        {
            DbContext = DbContext;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await DbContext.Users
                        .Select(u => u)
                        .Include(u => u.Client)
                        .OrderByDescending((u => u.ID))
                        //.OrderByDescending((u => u.CreateAt))
                        .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAll(Guid ClientID)
        {
            return await DbContext.Users
                        .Where(u => u.ClientID == ClientID)
                        .Include(u => u.Client)
                        .OrderByDescending((u => u.ID))
                        //.OrderByDescending((u => u.CreateAt))
                        .ToListAsync();
        }

        public async Task AddUser(User User)
        {
            await DbContext.Users.AddAsync(User);
            await DbContext.SaveChangesAsync();
        }
    }
}