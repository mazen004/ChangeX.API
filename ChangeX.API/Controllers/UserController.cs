using AutoMapper;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ChangeX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController( IMapper mapper, IUserServices userServices, IClientServices clientServices, ICurrentUserService currentUser) : ControllerBase
    {

        [HttpGet("GetAllUsers"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? query, [FromQuery] bool? systemRole)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(query))
                {
                    query = query.Trim();
                }

                Expression<Func<User, bool>>? predicate = null;

                if (!string.IsNullOrWhiteSpace(query) || systemRole.HasValue)
                {
                    predicate = u =>
                        (
                            string.IsNullOrWhiteSpace(query) ||
                            u.Name.Contains(query) ||
                            u.Email.Contains(query)
                        ) && (
                            !systemRole.HasValue ||
                            u.SystemRole == systemRole.Value
                        );
                }

                var users = await userServices.GetAll(predicate);

                if (!users.Success)
                {
                    return StatusCode(users.StatusCode, users.Message);
                }

                var data = mapper.Map<IEnumerable<UserAccountDto>>(users.Data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        
        [HttpGet("GetAllUsersClient/{ClientID:Guid}")]
        public async Task<IActionResult> GetAllUsersClient(Guid ClientID, [FromQuery] string? query)
        {
            try
            {
                if (currentUser.Role != "Admin" && currentUser.ClientId != ClientID)
                {
                    return Unauthorized();
                }

                var clientResult = await clientServices.GetByID(ClientID);

                if (!clientResult.Success)
                {
                    return StatusCode(
                        clientResult.StatusCode,
                        new
                        {
                            message = clientResult.Message
                        });
                }

                Expression<Func<User, bool>>? predicate = null;

                if (!string.IsNullOrWhiteSpace(query))
                {
                    query = query.Trim();

                    predicate = u =>
                        u.Name.Contains(query) ||
                        u.Email.Contains(query);
                }

                var users = await userServices.GetAll(ClientID, predicate);

                if (!users.Success || users.Data == null)
                {
                    return NotFound(new
                    {
                        message = "Users not found."
                    });
                }

                var data = mapper.Map<IEnumerable<UserInClientDto>>(users.Data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("GetUser/{ID:Guid}")]
        public async Task<IActionResult> GetUser(Guid ID)
        {
            try
            {
                if (currentUser.Role == "User" && currentUser.UserId != ID)
                {
                    return Unauthorized();
                }

                var user = await userServices.GetByID(ID);

                if(currentUser.Role == "UserAdmin" && user.Data?.ClientID != currentUser.ClientId)
                {
                    return Unauthorized();
                }

                if (!user.Success)
                {
                    return StatusCode(user.StatusCode, new { message = user.Message });
                }

                var data = mapper.Map<UserAccountDto>(user.Data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUserDto userDto)
        {
            try
            {
                var inputUser = mapper.Map<User>(userDto);

                var addedUser = await userServices.AddUser(inputUser);

                if (!addedUser.Success || addedUser.Data == null)
                {
                    return StatusCode(
                        addedUser.StatusCode,
                        new
                        {
                            message = addedUser.Message
                        });
                }

                var data = mapper.Map<UserAccountDto>(addedUser.Data);

                return Ok(new
                {
                    message = addedUser.Message,
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid ID, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                var user = await userServices.GetByID(ID);

                if (!user.Success)
                {
                    return StatusCode( user.StatusCode, new { message = user.Message });
                }

                var clientResult = await clientServices.GetByID(userDto.ClientID);

                if (!clientResult.Success)
                {
                    return StatusCode(clientResult.StatusCode, new { message = clientResult.Message });
                }

                mapper.Map(userDto, user.Data);

                await userServices.UpdateUser(user.Data);

                var data = mapper.Map<UserAccountDto>(user.Data);

                return Ok(new { message = "User updated successfully.", data});
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid ID)
        {
            try
            {
                var user = await userServices.GetByID(ID);

                if (!user.Success || user.Data == null)
                {

                    return StatusCode(user.StatusCode, new { message = user.Message });
                }

                await userServices.DeleteUser(user.Data);

                return Ok(new
                {
                    message = "User deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
    }
}