using RealQR_API.Models;
using RealQR_API.Repositories;

namespace RealQR_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<IEnumerable<User>> GetUsers() => await _userRepository.GetAllAsync();

        public async Task<User> GetUserById(int id) => await _userRepository.GetByIdAsync(id);

        public async Task<User> AddUser(User user) => await _userRepository.AddAsync(user);

        public async Task<bool> EditUser(User user, int id) => await _userRepository.EditAsync(user, id);

        public async Task<bool> DeleteUser(int id) => await _userRepository.DeleteAsync(id);
    }
}
