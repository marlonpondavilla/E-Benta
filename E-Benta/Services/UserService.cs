using E_Benta.Data;
using E_Benta.Dtos;
using E_Benta.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Services
{
    public class UserService(AppDbContext context) : IUserService
    {

        public Task<UserResponseDto> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserResponseDto>> GetUsersAsync()
            => await context.Users
                .Select(user => new UserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    isBentador = user.isBentador,
                })
                .ToListAsync();

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var result = await context.Users.Where(u => u.Id == id)
                .Select(user => new UserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    isBentador = user.isBentador,
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public Task<bool> UpdateUserAsync(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
