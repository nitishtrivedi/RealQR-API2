using RealQR_API.Models;

namespace RealQR_API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task<User> AddUser(User user);
        Task<bool> EditUser(User user, int id);
        Task<bool> DeleteUser(int id);
    }
}
