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
                        //.OrderByDescending((u => u.CreateAt))
                        .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAll(Guid ClientID)
        {
            return await _dbContext.Users
                        .AsNoTracking()
                        .Where(u => u.ClientID == ClientID)
                        .Include(u => u.Client)
                        //.OrderByDescending((u => u.CreateAt))
                        .ToListAsync();
        }

        public async Task<User> GetByID(Guid ID)
        {
            var user = await _dbContext.Users
                        .Where(u => u.ID == ID)
                        .Include(u => u.Client)
                        .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception($"User not found.");
                
            return user;
        }

        public Task<User> GetUserByEmailAndPassword(string Email, string Password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateUser(User User)
        {
            _dbContext.Users.Update(User);
            await _dbContext.SaveChangesAsync();
            return User;
        }

        public async Task AddUser(User User)
        {
            await _dbContext.Users.AddAsync(User);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CouldBeDefault(Guid ClientID)
        {
            return await _dbContext.Users.AnyAsync(c => c.ID == ClientID && c.IsPrimaryContact);
        }

        public async Task<bool> IsClientVailed(Guid ClientID)
        {
            return await _dbContext.Clients.AnyAsync(c => c.ID == ClientID);
        }
    }
}
