using E_Benta.Dtos;
using E_Benta.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Benta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetUsers()
            => Ok(await service.GetUsersAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await service.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound("User with the given id was not found.");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto newUser)
        {
            var createdUser = await service.CreateUserAsync(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, UpdateUserDto updatedUser)
        {
            var isUpdated = await service.UpdateUserAsync(id, updatedUser);
            if (!isUpdated)
            {
                return NotFound("User with the given id was not found.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var isDeleted = await service.DeleteUserAsync(id);
            if (!isDeleted)
            {
                return NotFound("User with the given id was not found.");
            }
            return NoContent();
        }

    }
}
