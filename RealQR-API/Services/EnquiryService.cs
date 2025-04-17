using Microsoft.EntityFrameworkCore;
using RealQR_API.DBContext;
using RealQR_API.Models;

namespace RealQR_API.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly RealQRDBContext _dbContext;
        public EnquiryService(RealQRDBContext dbContext) => _dbContext = dbContext;

        public async Task<List<Enquiry>> GetEnquiriesAsync()
        {
            return await _dbContext.Enquiry.ToListAsync();
        }

        public async Task<Enquiry> GetEnquiryAsync(int id)
        {
            return await _dbContext.Enquiry.FindAsync(id);
        }

        public async Task<Enquiry> AddEnquiryAsync(Enquiry enquiry)
        {
            _dbContext.Enquiry.Add(enquiry);
            await _dbContext.SaveChangesAsync();
            return enquiry;
        }

        public async Task<bool> EditEnquiryAsync(int id, Enquiry enquiry)
        {
            if (id != enquiry.Id) return false;
            var existingEnquiry = await _dbContext.Enquiry.FindAsync(id);
            if (existingEnquiry == null) return false;
            _dbContext.Entry(existingEnquiry).CurrentValues.SetValues(enquiry);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEnquiryAsync(int id)
        {
            var enquiry = await _dbContext.Enquiry.FindAsync(id);
            if (enquiry == null) return false;
            _dbContext.Enquiry.Remove(enquiry);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
