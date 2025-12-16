using E_Benta.Models;

namespace E_Benta.Services
{
    public class UserService : IUserService
    {

        static List<User> users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Username = "johndoe", PasswordHash = "hashedpassword1", isBentador = true },
            new User { Id = 2, Name = "Jane Smith", Username = "janesmith", PasswordHash = "hashedpassword2", isBentador = false },
            new User { Id = 3, Name = "Marlon Pondavilla", Username = "marlonpondavilla", PasswordHash = "hashedpassword3", isBentador = true },

        };

        public Task<User> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var result = users.FirstOrDefault(u => u.Id == id);
            return await Task.FromResult(result);
        }

        public async Task<List<User>> GetUsersAsync()
            => await Task.FromResult(users);

        public Task<bool> UpdateUserAsync(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
