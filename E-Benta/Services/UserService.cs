using E_Benta.Data;
using E_Benta.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Services
{
    public class UserService(AppDbContext context) : IUserService
    {

        public Task<User> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetUsersAsync()
            => await context.Users.ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var result = await context.Users.FindAsync(id);
            return result;
        }

        public Task<bool> UpdateUserAsync(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
