using E_Benta.Models;
using E_Benta.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Benta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
            => Ok(await service.GetUsersAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await service.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound("User with the given id was not found.");
            }

            return Ok(user);
        }

    }
}
