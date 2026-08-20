using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChangeX.BLL.Services
{
    public sealed class AuthService(ApplicationContext dbContex, IConfiguration configuration) : IAuthService
    {
        public async Task<string> Login(User User)
        {
            var user = await dbContex.Users
                        .Where(u => u.Email == User.Email)
                        .Include(u => u.Client)
                        .FirstOrDefaultAsync();

            if (user is null)
                throw new Exception($"Email is incorrect.");
            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, User.Password) == PasswordVerificationResult.Failed)
                throw new Exception($"Password is incorrect.");

            return CreateToken(user);
        }

        private string CreateToken(User User)
        {
            var IAdmin = User.SystemRole ? "Admin" : "User";
            var Clamis = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, User.ID.ToString()),
                new Claim (ClaimTypes.Name, User.Name),
                new Claim (ClaimTypes.Email, User.Email),
                new Claim (ClaimTypes.Role, IAdmin),
            };

            var Key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512);

            var TokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: Clamis,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: Creds
                );

            return new JwtSecurityTokenHandler().WriteToken(TokenDescriptor);
        }
    }
}