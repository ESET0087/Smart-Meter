


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

        //  REGISTER
        
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
                Displayname = request.DisplayName,
                Email = request.Email,
                Phone = request.Phone
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return new RegistrationResult { Success = true };
        }

        
        //  LOGIN with Lockout Tracking
      
        public async Task<string?> LoginAsync(LoginRequest login)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == login.Username);

            if (user == null || !user.Isactive)
                return null;

            //  Check if user is currently locked out
            if (user.LockoutEndUtc.HasValue && user.LockoutEndUtc > DateTime.UtcNow)
            {
                TimeSpan remaining = user.LockoutEndUtc.Value - DateTime.UtcNow;
                throw new Exception($"Account locked. Try again in {remaining.Minutes} minute(s).");
            }

            //  Verify password
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(login.Password, Encoding.UTF8.GetString(user.Passwordhash));

            if (!isPasswordCorrect)
            {
                user.FailedLoginCount++;
                user.LastFailedLoginUtc = DateTime.UtcNow;

                //  Lock the account after 5 failed attempts
                if (user.FailedLoginCount >= 5)
                {
                    user.LockoutEndUtc = DateTime.UtcNow.AddMinutes(15);
                    await _context.SaveChangesAsync();
                    throw new Exception("Too many failed attempts. Account locked for 15 minutes.");
                }

                await _context.SaveChangesAsync();
                throw new Exception($"Invalid password. Attempt {user.FailedLoginCount} of 5.");
            }

            //  Success: reset counters and lockout
            user.FailedLoginCount = 0;
            user.LockoutEndUtc = null;
            user.Lastloginutc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            //  Generate JWT
            return GenerateToken(user);
        }

      
        //  TOKEN GENERATION
        
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
