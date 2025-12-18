using E_Benta.Dtos;
using E_Benta.Models;

namespace E_Benta.Services
{
    public interface IUserService
    {
        Task<List <UserResponseDto  >> GetUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<UserResponseDto> CreateUserAsync(CreateUserDto newUser);

        Task<bool> UpdateUserAsync(int id, UpdateUserDto updatedUser);
        Task<bool> DeleteUserAsync(int id);

        Task<LoginResponseDto?> LoginAsync(LoginUserDto loginDto);

    }
}
