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
    public class UserController(
        IMapper mapper,
        IUserServices userServices,
        IClientServices clientServices,
        ICurrentUserService currentUser) : ControllerBase
    {
        // =========================================================
        // GET ALL USERS
        // Admin only
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] string? query,
            [FromQuery] bool? systemRole)
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
                        )
                        &&
                        (
                            !systemRole.HasValue ||
                            u.SystemRole == systemRole.Value
                        );
                }

                var users = await userServices.GetAll(predicate);

                if (!users.Success || users.Data == null)
                {
                    return NotFound(new
                    {
                        message = "Users not found."
                    });
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


        // =========================================================
        // GET ALL USERS OF A CLIENT
        //
        // Admin:
        //      Can request any ClientID
        //
        // User:
        //      Can only request users from his own ClientID
        // =========================================================
        [HttpGet("GetAllUsersClient/{ClientID:Guid}")]
        public async Task<IActionResult> GetAllUsersClient(
            Guid ClientID,
            [FromQuery] string? query)
        {
            try
            {
                // Normal user can only access his own client
                if (currentUser.Role != "Admin" &&
                    currentUser.ClientId != ClientID)
                {
                    return Forbid();
                }

                var clientResult = await clientServices.GetByID(ClientID);

                if (!clientResult.Success || clientResult.Data == null)
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


        // =========================================================
        // GET USER BY ID
        //
        // Admin:
        //      Can get any user
        //
        // User:
        //      Can only get himself
        // =========================================================
        [HttpGet("GetUser/{ID:Guid}")]
        public async Task<IActionResult> GetUser(Guid ID)
        {
            try
            {
                // Normal user can only access his own account
                if (currentUser.Role != "Admin" &&
                    currentUser.UserId != ID)
                {
                    return Forbid();
                }

                var user = await userServices.GetByID(ID);

                if (!user.Success || user.Data == null)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
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


        // =========================================================
        // ADD USER
        // Admin only
        // =========================================================
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

                // Never return User entity directly.
                // It contains the password hash.
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


        // =========================================================
        // UPDATE USER
        // Admin only
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(
            [FromQuery] Guid ID,
            UpdateUserDto userDto)
        {
            try
            {
                var user = await userServices.GetByID(ID);

                if (!user.Success || user.Data == null)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
                }

                // Make sure the selected Client exists
                var clientResult = await clientServices.GetByID(userDto.ClientID);

                if (!clientResult.Success || clientResult.Data == null)
                {
                    return StatusCode(
                        clientResult.StatusCode,
                        new
                        {
                            message = clientResult.Message
                        });
                }

                mapper.Map(userDto, user.Data);

                await userServices.UpdateUser(user.Data);

                // Never return User entity directly
                var data = mapper.Map<UserAccountDto>(user.Data);

                return Ok(new
                {
                    message = "User updated successfully.",
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



        // =========================================================
        // DELETE USER
        // Admin only
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{ID:Guid}")]
        public async Task<IActionResult> DeleteUser(Guid ID)
        {
            try
            {
                var user = await userServices.GetByID(ID);

                if (!user.Success || user.Data == null)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
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