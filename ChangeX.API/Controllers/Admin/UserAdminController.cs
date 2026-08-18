using ChangeX.BLL.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using ChangeX.BLL.Services;
using AutoMapper;
using ChangeX.DAL.Database;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly ApplicationContext DbContext;
        private readonly IMapper Mapper;
        private readonly IUserServices UserServices;

        public UserAdminController(IMapper mapper, IUserServices userServices, ApplicationContext dbContext)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
            this.UserServices = userServices;

        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var User = await UserServices.GetAll();

            var Data = Mapper.Map<IEnumerable<UserInClientDto>>(User);

            return Ok(Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto userDto)
        {
            var user = new DAL.Entities.User()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                // Password = userDto.Password,
                SystemRole = userDto.SystemRole,
                IsPrimaryContact = userDto.IsPrimaryContact,
                ClientID = userDto.ClientID
            };

            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            return Ok(new { message = "User added successfully", data = user });
        }
    }
}
