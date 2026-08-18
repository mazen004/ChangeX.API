using ChangeX.DAL.Entities;
using ChangeX.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationContext _dbContext;

        public UserServices(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users
                        .AsNoTracking()
                        .Include(u => u.Client)
                        .OrderByDescending((u => u.ID))
                        //.OrderByDescending((u => u.CreateAt))
                        .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAll(Guid ClientID)
        {
            return await _dbContext.Users
                        .AsNoTracking()
                        .Where(u => u.ClientID == ClientID)
                        .Include(u => u.Client)
                        .OrderByDescending((u => u.ID))
                        //.OrderByDescending((u => u.CreateAt))
                        .ToListAsync();
        }

        public async Task AddUser(User User)
        {
            var clientExists = await _dbContext.Clients
                .AsNoTracking()
                .AnyAsync(client => client.ID == User.ClientID);

            if (!clientExists)
            {
                throw new KeyNotFoundException("Client not found");
            }

            await _dbContext.Users.AddAsync(User);
            await _dbContext.SaveChangesAsync();
        }
    }
}
