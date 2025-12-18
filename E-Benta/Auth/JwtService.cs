using E_Benta.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace E_Benta.Auth
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }

    public class JwtService : IJwtService
    {
        private readonly byte[] _keyBytes;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            var keyString = configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(keyString))
                throw new InvalidOperationException("Jwt:Key is missing. Put it in User Secrets or environment variables.");

            try
            {
                _keyBytes = Convert.FromBase64String(keyString);
            }
            catch
            {
                _keyBytes = Encoding.UTF8.GetBytes(keyString);
            }

            if (_keyBytes.Length < 16)
                throw new InvalidOperationException("Jwt:Key must be at least 128 bits (16 bytes). Use a 32-byte key for HS256.");

            _issuer = configuration["Jwt:Issuer"] ?? "app";
            _audience = configuration["Jwt:Audience"] ?? "app";
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("name", user.Name),
                new Claim("isBentador", user.isBentador.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(_keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
