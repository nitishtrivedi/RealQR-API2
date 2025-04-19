using Microsoft.IdentityModel.Tokens;
using RealQR_API.DBContext;
using RealQR_API.Models;
using RealQR_API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealQR_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        public async Task<string> Register(string username, string firstname, string lastname, string password, string email, bool isUserAdmin)
        {
            var returnedUser = await _authRepository.RegisterAsync(username, firstname, lastname, password, email, isUserAdmin);
            return GenerateJwtToken(returnedUser);
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _authRepository.LoginAsync(username, password);
            return GenerateJwtToken(user);
        }


        

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, user.IsUserAdmin ? "Admin" : "User"),
                new Claim(ClaimTypes.Name, user.Id.ToString()),
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
