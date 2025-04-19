using Microsoft.EntityFrameworkCore;
using RealQR_API.DBContext;
using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly RealQRDBContext _dbContext;
        public EnquiryRepository(RealQRDBContext dbContext) => _dbContext = dbContext;


        public async Task<IEnumerable<Enquiry>> GetAllAsync() => await _dbContext.Enquiry.ToListAsync();

        public async Task<Enquiry> GetAsync(int id) => await _dbContext.Enquiry.FindAsync(id);

        public async Task<Enquiry> AddAsync(Enquiry enquiry)
        {
            _dbContext.Enquiry.Add(enquiry);
            await _dbContext.SaveChangesAsync();
            return enquiry;
        }

        public async Task<bool> EditAsync(int id, Enquiry enquiry)
        {
            if (id != enquiry.Id) return false;
            var existingEnquiry = await _dbContext.Enquiry.FindAsync(id);
            if (existingEnquiry == null) return false;
            _dbContext.Entry(existingEnquiry).CurrentValues.SetValues(enquiry);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var enquiry = await _dbContext.Enquiry.FindAsync(id);
            if (enquiry == null) return false;
            _dbContext.Enquiry.Remove(enquiry);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
