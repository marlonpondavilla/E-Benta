using E_Benta.Data;
using E_Benta.Dtos;
using E_Benta.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Services
{
    public class UserService(AppDbContext context) : IUserService
    {

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Username = user.Username,
                PasswordHash = user.Password,
                isBentador = user.isBentador,
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Username = newUser.Username,
                isBentador = newUser.isBentador,
            };
        }

        public async Task<bool> DeleteUserAsync(int id)   
        {
            var userToDelete = await context.Users.FindAsync(id);
            if (userToDelete == null)
            {
                return false;
            }
            context.Users.Remove(userToDelete);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<UserResponseDto>> GetUsersAsync()
            => await context.Users
                .Select(user => new UserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    HashPassword = user.PasswordHash,
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
                    HashPassword = user.PasswordHash,
                    isBentador = user.isBentador,
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto updatedUser)
        {
            var existingUser = await context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Username = updatedUser.Username;
            existingUser.PasswordHash = updatedUser.HashPassword;
            existingUser.isBentador = updatedUser.isBentador;

            await context.SaveChangesAsync();
            return true;
        }
    }
}
