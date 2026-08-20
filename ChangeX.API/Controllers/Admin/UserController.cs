using ChangeX.DAL.Entities;
using ChangeX.BLL.Services;
using ChangeX.BLL.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ChangeX.BLL.Interfaces;
//using ChangeX.BLL.Interfaces;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMapper mapper, IUserServices userServices, IAuthService authService, IClientServices clientServices) : ControllerBase
    {

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? search)
        {
            try
            {
                var users = await userServices.GetAll(search);
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
        public async Task<IActionResult> GetALLUsers(Guid ClientID)
        {
            try
            {
                var users = await userServices.GetAll(ClientID);
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
        public async Task<IActionResult> Login([FromQuery] string Email,[FromQuery] string Password)
        {
            try
            {
                var token = await authService.Login(Email, Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserDto UserDto)
        {
            try
            {
                if (!await userServices.IsClientVailed(UserDto.ClientID))
                    throw new Exception("InVailed Client ID");

                if (!await userServices.CouldBeDefault(UserDto.ClientID))
                    throw new Exception("Only one Default Contact per Client");

                var User = mapper.Map<User>(UserDto);

                await userServices.AddUser(User);

                return Ok(User);
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

                if (!await userServices.IsClientVailed(UserDto.ClientID))
                    throw new Exception("InVailed Client ID");

                if (!await userServices.CouldBeDefault(UserDto.ClientID))
                    throw new Exception("Only one Default Contact per Client");

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
