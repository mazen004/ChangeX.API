using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChangeX.BLL.DTOs;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using ChangeX.BLL.Interfaces;

namespace ChangeX.BLL.Services
{
    public class UserServices(ApplicationContext dbContex) : IUserServices
    {

        public async Task<ServiceResponse<IEnumerable<User>>> GetAll(Expression<Func<User, bool>>? predicate)
        {
            var users = dbContex.Users
                .AsNoTracking()
                .Include(u => u.Client)
                .AsQueryable();

            if (predicate != null)
            {
                users = users.Where(predicate);
            }

            return ServiceResponse<IEnumerable<User>>.Ok(await users.ToListAsync(), "Get All Users");
        }

        public async Task<ServiceResponse<IEnumerable<User>>> GetAll(Guid ClientID, Expression<Func<User, bool>>? predicate)
        {
            var users = dbContex.Users
                        .AsNoTracking()
                        .Where(u => u.ClientID == ClientID)
                        .Include(u => u.Client);

            if (predicate != null)
            {

                users = users.Where(predicate)
                    .Include(u => u.Client);
            }

            return ServiceResponse<IEnumerable<User>>.Ok(await users.ToListAsync(), "Get All Users in Client");
        }

        public async Task<ServiceResponse<User>> GetByID(Guid ID)
        {
            var user = await dbContex.Users
                        .Where(u => u.ID == ID)
                        .Include(u => u.Client)
                        .FirstOrDefaultAsync();

            if (user == null)
            {
                return ServiceResponse<User>.Fail("User not found", 404);
            }

            return ServiceResponse<User>.Ok(user, "User found");
        }

        public async Task<ServiceResponse<User>> UpdateUser(User User)
        {
            var getUserResponse = await GetByID(User.ID);
            if (getUserResponse.Data == null)
            {
                return ServiceResponse<User>.Fail("User not found", 404);
            }

            User.Password = new PasswordHasher<User>().HashPassword(User, User.Password);
            dbContex.Users.Update(User);
            await dbContex.SaveChangesAsync();

            return ServiceResponse<User>.Ok((await GetByID(User.ID)).Data, "User updated");
        }

        public async Task<ServiceResponse<User>> AddUser(User User)
        {
            var isUserExists = await IsUserExists(User.Email, User.PhoneNumber);
            if (isUserExists.Data)
            {
                return ServiceResponse<User>.Fail("User already exists", 400);
            }

            User.Password = new PasswordHasher<User>().HashPassword(User, User.Password);
            await dbContex.Users.AddAsync(User);
            await dbContex.SaveChangesAsync();
            return ServiceResponse<User>.Ok((await GetByID(User.ID)).Data, "User added");
        }

        public async Task<ServiceResponse<bool>> DeleteUser(User User)
        {
            var isUserExists = await GetByID(User.ID);
            if (isUserExists.Data == null)
            {
                return ServiceResponse<bool>.Fail("User Already Does Not Exist", 404);
            }

            dbContex.Users.Remove(User);
            await dbContex.Clients.Where(c => c.ID == User.ClientID && c.DefaultContactID == User.ID).ForEachAsync(c => c.DefaultContactID = null);
            await dbContex.SaveChangesAsync();
            return ServiceResponse<bool>.Ok(true, "User deleted");
        }

        public async Task<ServiceResponse<bool>> IsInClient(Guid ClientID, Guid UserID)
        {
            var isDefaultContact = await dbContex.Clients
                        .AnyAsync(c => c.ID == ClientID && c.DefaultContactID == UserID);

            if (!isDefaultContact)
                return ServiceResponse<bool>.Fail("not a Default Contact in Client", 404);

            return ServiceResponse<bool>.Ok(true, "User is a Default Contact in Client");
        }

        public async Task<ServiceResponse<bool>> IsUserExists(string Email, string PhoneNumber)
        {
            var userExists = await dbContex.Users.AnyAsync(u => u.Email == Email || u.PhoneNumber == PhoneNumber);

            if (!userExists)
                return ServiceResponse<bool>.Fail("User not found", 404);

            return ServiceResponse<bool>.Ok(userExists, "User found");
        }
    }
}
