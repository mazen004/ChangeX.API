using ChangeX.DAL.Entities;
using ChangeX.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.Services
{
    public class UserServices(ApplicationContext dbContex) : IUserServices
    {

        public async Task<IEnumerable<User>> GetAll(string? search = null)
        {
            var query = dbContex.Users
                .AsNoTracking()
                .Include(u => u.Client)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(u =>
                    u.Name.Contains(search) ||
                    u.Email.Contains(search))
                    .Include(u => u.Client);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAll(Guid ClientID, string? search = null)
        {
            var query = dbContex.Users
                        .AsNoTracking()
                        .Where(u => u.ClientID == ClientID)
                        .Include(u => u.Client);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(u =>
                    u.Name.Contains(search) ||
                    u.Email.Contains(search))
                    .Include(u => u.Client);
            }

            return await query.ToListAsync();
        }

        public async Task<User> GetByID(Guid ID)
        {
            var user = await dbContex.Users
                        .Where(u => u.ID == ID)
                        .Include(u => u.Client)
                        .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception($"User not found.");
                
            return user;
        }

        public async Task<User> UpdateUser(User User)
        {
            dbContex.Users.Update(User);
            await dbContex.SaveChangesAsync();
            return User;
        }

        public async Task AddUser(User User)
        {
            await dbContex.Users.AddAsync(User);
            await dbContex.SaveChangesAsync();
        }

        public async Task<bool> CouldBeDefault(Guid ClientID)
        {
            return await dbContex.Users.AnyAsync(u => u.ClientID == ClientID && u.IsPrimaryContact);
        }

        public async Task<bool> IsUserFound(string Email)
        {
            return await dbContex.Users.AnyAsync(u => u.Email == Email);
        }

        public async Task DeleteUser(User User)
        {
            dbContex.Users.Remove(User);
            await dbContex.SaveChangesAsync();
        }
    }
}
