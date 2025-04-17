using Microsoft.IdentityModel.Tokens;
using RealQR_API.DBContext;
using RealQR_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealQR_API.Services
{
    public class UserService : IUserService
    {
        private readonly RealQRDBContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(RealQRDBContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<string> Register(string username, string firstname, string lastname, string password, string email, bool isUserAdmin)
        {
            var user = new User
            {
                UserName = username,
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                IsUserAdmin = isUserAdmin
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return GenerateJwtToken(user);
        }

        public async Task<string> Login(string username, string password)
        {
            var user =  _dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    throw new Exception("Invalid Password");
                }
            }
            else
            {
                throw new Exception("Invalid User or User not found");
            }
            return GenerateJwtToken(user);
        }


        

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, user.IsUserAdmin ? "Admin" : "User")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
