using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ChangeX.DAL.Entities;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;

namespace ChangeX.API.Controllers
{
    [Route("api/Auth/Login")]
    [ApiController]

    public class AuthorizeLoginController(IMapper mapper, IAuthService authService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await authService.Login(mapper.Map<User>(loginDto));
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerException = ex.InnerException?.Message });
            }
        }
    }
}
