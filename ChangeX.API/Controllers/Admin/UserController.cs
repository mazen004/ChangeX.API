using ChangeX.DAL.Entities;
using ChangeX.BLL.Services;
using ChangeX.BLL.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public UserController(IMapper mapper, IUserServices userServices)
        {
            _mapper = mapper;
            _userServices = userServices;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userServices.GetAll();
                if (users == null)
                    return NotFound(new { message = "Users not found." });

                var data = _mapper.Map<IEnumerable<UserAccountDto>>(users);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllUsersClient/{ClientID:Guid}")]
        public async Task<IActionResult> GetALLUsers(Guid ClientID)
        {
            try
            {
                if(!await _userServices.IsClientVailed(ClientID))
                    return NotFound(new { message = "Client Not Found" });
                var users = await _userServices.GetAll(ClientID);
                var data = _mapper.Map<IEnumerable<UserAccountDto>>(users);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{ID:Guid}")]
        public async Task<IActionResult> GetUser(Guid ID)
        {
            try
            {
                var user = await _userServices.GetByID(ID);
                if(user == null)
                    return NotFound(new { message = "User not found." });
                var data = _mapper.Map<UserAccountDto>(user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("Login/{Email}/{Password}")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            return NotFound(new { message = "Login is not implemented." });
            try
            {
                var user = await _userServices.Login(Email, Password);
                if (user == null)
                    return NotFound(new { message = "User Email or Password is incorrect." });
                var data = _mapper.Map<UserAccountDto>(user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto UserDto)
        {
            try
            {
                if (!await _userServices.IsClientVailed(UserDto.ClientID))
                    return NotFound(new { message = "Client Not Found" });

                if (!await _userServices.CouldBeDefault(UserDto.ClientID))
                    return BadRequest(new { message = "Only one Default Contact per Client" });

                var User = _mapper.Map<User>(UserDto);

                await _userServices.AddUser(User);

                return Ok(User);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("UpdateUser/{ID:Guid}")]
        public async Task<IActionResult> UpdateUser(Guid ID, UpdateUserDto UserDto)
        {
            try
            {
                var user = await _userServices.GetByID(ID);

                if (user == null)
                    return NotFound(new { message = "User not found." });

                if (!await _userServices.IsClientVailed(UserDto.ClientID))
                    return NotFound(new { message = "Client Not Found" });

                if (!await _userServices.CouldBeDefault(UserDto.ClientID))
                    return BadRequest(new { message = "Only one Default Contact per Client" });

                _mapper.Map(UserDto, user);

                await _userServices.UpdateUser(user);

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
                var user = await _userServices.GetByID(ID);

                if (user == null)
                    return NotFound(new { message = "User not found." });

                await _userServices.DeleteUser(user);

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
