using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using ChangeX.BLL.DTOs;

namespace ChangeX.BLL.Services
{
    public sealed class AuthService(ApplicationContext dbContex, IConfiguration configuration, IUserServices userServices) : IAuthService
    {
        public async Task<ServiceResponse<string>> Login(User User)
        {
            var user = await dbContex.Users
                        .Where(u => u.Email == User.Email)
                        .Include(u => u.Client)
                        .FirstOrDefaultAsync();

            if (user is null)
                return ServiceResponse<string> .Fail(" Login Failes: Email is incorrect.", 404 );
            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, User.Password) == PasswordVerificationResult.Failed)
                return ServiceResponse<string> .Fail(" Login Failes: Password is incorrect.", 404 );

            var token = await CreateToken(user);
            return ServiceResponse<string>.Ok(token, "Login successful. Role: " + await LoginRole(user) );
        }

        private async Task<string> LoginRole(User User)
        {

            var IsUserAdmin = await userServices.IsInClient(User.ClientID, User.ID);

            return User.SystemRole ? "Admin"
                : IsUserAdmin.Data ? "UserAdmin"
                : "User";
        }

        private async Task<string> CreateToken(User User)
        {
            var Role = await LoginRole(User);

            var Clamis = new List<Claim>
            {
                new Claim ("UserID", User.ID.ToString()),
                new Claim (ClaimTypes.Name, User.Name),
                new Claim (ClaimTypes.Email, User.Email),
                new Claim ("PhoneNumber", User.PhoneNumber),
                new Claim (ClaimTypes.Role, Role),
                new Claim("ClientID", User.ClientID.ToString())
            };

            var Key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512);

            var TokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: Clamis,
                expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("AppSettings:ExpireTime")),
                signingCredentials: Creds
                );

            return new JwtSecurityTokenHandler().WriteToken(TokenDescriptor);
        }
    }
}