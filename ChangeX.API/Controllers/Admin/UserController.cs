using AutoMapper;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IMapper mapper, IUserServices userServices,IClientServices clientServices) : ControllerBase
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
                                      && (!systemRole.HasValue || u.SystemRole == systemRole.Value));
                }

                var users = await userServices.GetAll(predicate);
                if (users == null)
                    return NotFound("Users not found.");
                var data = mapper.Map<IEnumerable<UserAccountDto>>(users);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerExeption = ex.InnerException });
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
                var data = mapper.Map<IEnumerable<UserInClientDto>>(users);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerExeption = ex.InnerException });
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
                return BadRequest(new { message = ex.Message, innerExeption = ex.InnerException });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserDto User)
        {
            try
            {
                var InputUser = mapper.Map<User>(User);

                var AddedUser =  await userServices.AddUser(InputUser);

                if(!AddedUser.Success)
                    return StatusCode(AddedUser.StatusCode, new { message = AddedUser.Message });

                return Ok(new {message = AddedUser.Message, AddedUser.Data});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerExeption =  ex.InnerException});
            }
        }
        
        [Authorize]
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

                mapper.Map(UserDto, user.Data);

                await userServices.UpdateUser(user.Data);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerExeption = ex.InnerException });
            }
        }

        [Authorize]
        [HttpDelete("DeleteUser/{ID:Guid}")]
        public async Task<IActionResult> DeleteUser(Guid ID)
        {
            try
            {
                var user = await userServices.GetByID(ID);

                if (user == null)
                    return NotFound("User not found.");

                await userServices.DeleteUser(user.Data);

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerExeption = ex.InnerException });
            }
        }
    }
}
