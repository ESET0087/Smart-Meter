using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using smart_meter.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using smart_meter.Model.DTOs;
using smart_meter.Data.Context;

namespace smart_meter.Services
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class AuthService
    {
        private readonly AppDbContext _context; 
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<RegistrationResult> RegisterAsync(RegisterRequest request)
        {
            if (await _context.User.AnyAsync(u => u.Username == request.Username))
            {
                return new RegistrationResult { Success = false, Message = "Username already taken." };
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Passwordhash = Encoding.UTF8.GetBytes(passwordHash),
                Displayname = request.DisplayName
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return new RegistrationResult { Success = true };
        }

        public async Task<string?> LoginAsync(LoginRequest login)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == login.Username);

            if (user == null || !user.Isactive)
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(login.Password, Encoding.UTF8.GetString(user.Passwordhash)))
            {
                return null;
            }

            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.NameId, user.Userid.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}