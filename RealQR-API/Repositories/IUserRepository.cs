using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<bool> EditAsync(User user, int id);
        Task<bool> DeleteAsync(int id);
    }
}
