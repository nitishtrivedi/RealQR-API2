using RealQR_API.DBContext;
using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(string username, string firstname, string lastname, string password, string email, bool isUserAdmin);
        Task<User> LoginAsync(string username, string password);
    }
}
