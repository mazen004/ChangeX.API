using System;
using ChangeX.DAL.Database;
using ChangeX.BLL.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly ApplicationContext DbContext;

        public UserAdminController(ApplicationContext DbContext)
        {
            this.DbContext = DbContext;
        }

        [HttpGet]
        public IActionResult GetUserAdmin()
        {
            return Ok(new { message = "Get all User for Admin", data = DbContext.Users.ToList()});
        }

        [HttpPost]
        public IActionResult AddUser(AddUserDto userDto)
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

            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            return Ok(new { message = "User added successfully", data = user });
        }
    }
}
