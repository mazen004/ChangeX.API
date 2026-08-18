using ChangeX.BLL.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.Services;
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
            var data = _mapper.Map<IEnumerable<UserInClientDto>>(users);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto userDto)
        {
            var user = new DAL.Entities.User()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                SystemRole = userDto.SystemRole,
                IsPrimaryContact = userDto.IsPrimaryContact,
                ClientID = userDto.ClientID
            };

            try
            {
                await _userServices.AddUser(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "User added successfully", data = user });
        }
    }
}
