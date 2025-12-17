using E_Benta.Dtos;
using E_Benta.Models;

namespace E_Benta.Services
{
    public interface IUserService
    {
        Task<List <UserResponseDto  >> GetUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<UserResponseDto> CreateUserAsync(User user);

        Task<bool> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);

    }
}
