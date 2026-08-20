using AutoMapper;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;
using ChangeX.BLL.Services;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
//using ChangeX.BLL.Interfaces;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMapper mapper, IUserServices userServices, IAuthService authService, IClientServices clientServices) : ControllerBase
    {
        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? query, bool? systemRole)
        {
            try
            {
                Expression<Func<User, bool>>? predicate = null;

                if (!string.IsNullOrWhiteSpace(query) || systemRole.HasValue)
                {
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        query = query.Trim();
                    }
                    predicate = u => (query != null && (u.Name.Contains(query) || u.Email.Contains(query))
                                      && (systemRole.HasValue && u.SystemRole == systemRole.Value));
                }

                var users = await userServices.GetAll(predicate);
                if (users == null)
                    return NotFound("Users not found.");
                var data = mapper.Map<IEnumerable<UserAccountDto>>(users);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [Authorize]
        [HttpGet("GetAllUsersClient/{ClientID:Guid}")]
        public async Task<IActionResult> GetAllUsers(Guid ClientID, [FromQuery] string? query)
        {
            try
            {
                var clientResult = await clientServices.GetByID(ClientID);
                if (!clientResult.Success)
                    return StatusCode(clientResult.StatusCode, new { message = clientResult.Message });

                Expression<Func<User, bool>>? predicate = null;

                if (!string.IsNullOrWhiteSpace(query))
                {
                    query = query.Trim();
                    predicate = u => query != null && (u.Name.Contains(query) || u.Email.Contains(query));
                }
                var users = await userServices.GetAll(ClientID, predicate);
                if (users == null)
                    return NotFound("Users not found.");
                var data = mapper.Map<IEnumerable<UserAccountDto>>(users);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetUser/{ID:Guid}")]
        public async Task<IActionResult> GetUser(Guid ID)
        {
            try
            {
                var user = await userServices.GetByID(ID);
                if (user == null)
                    return NotFound("User not found.");
                var data = mapper.Map<UserAccountDto>(user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginDto loginDto)
        {
            try
            {
                var token = await authService.Login(mapper.Map<User>(loginDto));
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserDto User)
        {
            try
            {
                if (await userServices.IsUserFound(User.Email))
                    throw new Exception("User is already registered");

                //if (!await userServices.CouldBeDefault(User.ClientID))
                //    throw new Exception("Only one Default Contact per Client");

                var user = mapper.Map<User>(User);

                await userServices.AddUser(user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("UpdateUser/{ID:Guid}")]
        public async Task<IActionResult> UpdateUser(Guid ID, [FromForm] UpdateUserDto UserDto)
        {
            try
            {
                var user = await userServices.GetByID(ID);

                if (user == null)
                    return NotFound("User not found.");

                var clientResult = await clientServices.GetByID(UserDto.ClientID);
                if (!clientResult.Success)
                    return StatusCode(clientResult.StatusCode, new { message = clientResult.Message });

                //if (!await userServices.CouldBeDefault(UserDto.ClientID))
                //    throw new Exception("Only one Default Contact per Client");

                mapper.Map(UserDto, user);

                await userServices.UpdateUser(user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("DeleteUser/{ID:Guid}")]
        public async Task<IActionResult> DeleteUser(Guid ID)
        {
            try
            {
                var user = await userServices.GetByID(ID);

                if (user == null)
                    return NotFound("User not found.");

                await userServices.DeleteUser(user);

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
