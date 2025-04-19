using RealQR_API.DBContext;
using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly RealQRDBContext _dbContext;
        public AuthRepository(RealQRDBContext dbContext) => _dbContext = dbContext;

        public Task<User> LoginAsync(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == username);
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
            return Task.FromResult(user);
        }

        public async Task<User> RegisterAsync(string username, string firstname, string lastname, string password, string email, bool isUserAdmin)
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
            return user;
        }
    }
}
