using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ChangeX.DAL.Entities;
using ChangeX.BLL.DTOs.Users;
using ChangeX.BLL.Interfaces;

namespace ChangeX.API.Controllers
{
    [Route("api/Auth/Login")]
    [ApiController]
    public class AuthorizeLoginController(
        IMapper mapper,
        IAuthService authService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await authService.Login(
                    mapper.Map<User>(loginDto)
                );

                if (!result.Success || string.IsNullOrEmpty(result.Data))
                {
                    return StatusCode(
                        result.StatusCode,
                        new
                        {
                            message = result.Message
                        }
                    );
                }

                return Ok(new
                {
                    token = result.Data
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