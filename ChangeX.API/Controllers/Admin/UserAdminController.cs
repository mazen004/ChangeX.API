using ChangeX.DAL.Entities;
using ChangeX.BLL.Services;
using ChangeX.BLL.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public UserAdminController(IMapper mapper, IUserServices userServices)
        {
            _mapper = mapper;
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var users = await _userServices.GetAll();
            var data = _mapper.Map<IEnumerable<UserAccountDto>>(users);
            return Ok(data);
        }

        [HttpGet("{ID:Guid}")]
        public async Task<IActionResult> GetUser(Guid ID)
        {
            var user = await _userServices.GetByID(ID);
            var data = _mapper.Map<UserAccountDto>(user);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto UserDto)
        {
            try
            {
                if (!await _userServices.IsClientVailed(UserDto.ClientID))
                    throw new Exception("InVailed Client ID");

                if (!await _userServices.CouldBeDefault(UserDto.ClientID))
                    throw new Exception("Only one Default Contact per Client");

                var User = _mapper.Map<User>(UserDto);

                await _userServices.AddUser(User);

                return Ok(User);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{ID:Guid}")]
        public async Task<IActionResult> UpdateUser(Guid ID, UpdateUserDto UserDto)
        {
            try
            {
                var user = await _userServices.GetByID(ID);

                if (user == null)
                    throw new Exception("User not found.");

                if (!await _userServices.IsClientVailed(UserDto.ClientID))
                    throw new Exception("InVailed Client ID");

                if (!await _userServices.CouldBeDefault(UserDto.ClientID))
                    throw new Exception("Only one Default Contact per Client");

                _mapper.Map(UserDto, user);

                await _userServices.UpdateUser(user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
