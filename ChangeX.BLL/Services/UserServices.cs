using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChangeX.BLL.Services
{
    public class UserServices(ApplicationContext dbContex) : IUserServices
    {

        public async Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>>? predicate)
        {
            var query = dbContex.Users
                .AsNoTracking()
                .Include(u => u.Client)
                .AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAll(Guid ClientID, Expression<Func<User, bool>>? predicate)
        {
            var query = dbContex.Users
                        .AsNoTracking()
                        .Where(u => u.ClientID == ClientID)
                        .Include(u => u.Client);

            if (predicate != null)
            {

                query = query.Where(predicate)
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
            User.Password = new PasswordHasher<User>().HashPassword(User, User.Password);
            await dbContex.Users.AddAsync(User);
            await dbContex.SaveChangesAsync();
        }

        //public async Task<bool> CouldBeDefault(Guid ClientID)
        //{
        //    return await dbContex.Users.AnyAsync(u => u.ClientID == ClientID && u.IsPrimaryContact);
        //}

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
