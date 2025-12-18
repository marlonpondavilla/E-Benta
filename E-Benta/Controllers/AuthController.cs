using E_Benta.Dtos;
using E_Benta.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Benta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService service) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var result = await service.LoginAsync(loginDto);
            if(result == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(result);
        }
            

    }
}
