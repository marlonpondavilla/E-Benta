using E_Benta.Models;

namespace E_Benta.Services
{
    public interface IUserService
    {
        Task<List <User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);

        Task<bool> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);

    }
}
