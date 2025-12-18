using E_Benta.Data;
using E_Benta.Dtos;
using E_Benta.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Benta.Services
{
    public class UserService(AppDbContext context, IPasswordHasher<User> passwordHasher) : IUserService
    {

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Username = user.Username,
                isBentador = user.isBentador,
            };

            //hashing password
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, user.Password);

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

        public async Task<LoginResponseDto?> LoginAsync(LoginUserDto loginDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if(user == null)
            {
                return null;
            }

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if(result != PasswordVerificationResult.Success)
            {
                return null;
            }   

            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Name = user.Name,
                isBentador = user.isBentador,
            };

        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("name", user.Name),
                new Claim("isBentador", user.isBentador.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SECRET KEY"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "app",
                audience: "app",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
