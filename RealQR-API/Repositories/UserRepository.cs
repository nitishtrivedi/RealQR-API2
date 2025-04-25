using Microsoft.EntityFrameworkCore;
using RealQR_API.DBContext;
using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RealQRDBContext _dbContext;
        public UserRepository(RealQRDBContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            return user ?? throw new Exception("User Not Found");
        }

        public async Task<User> AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> EditAsync(User user, int id)
        {
            if (id != user.Id) return false;
            var existingUser = await _dbContext.Users.FindAsync(id);
            if (existingUser == null) return false;
            _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            //var user = await GetByIdAsync(id);
            //if (user == null) return false;
            //_dbContext.Users.Remove(user);
            //await _dbContext.SaveChangesAsync();
            //return true;

            var user = await _dbContext.Users
                .Include(u => u.Enquiries)
                .ThenInclude(e => e.EnquiryQuestionnaire)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;
            foreach (var enquiry in user.Enquiries)
            {
                enquiry.UserId = null;
                enquiry.User = null;
                if (enquiry.EnquiryQuestionnaire != null)
                {
                    enquiry.EnquiryQuestionnaire.AgentName = ""; // Clear AgentName
                }
            }

            _dbContext.Users.Remove(user);
            var result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }
    }
}
