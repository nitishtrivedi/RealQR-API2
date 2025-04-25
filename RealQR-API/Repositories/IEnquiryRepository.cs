using RealQR_API.DTOs;
using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public interface IEnquiryRepository
    {
        Task<IEnumerable<EnquiryDto>> GetAllAsync();
        Task<EnquiryDto> GetAsync(int id);
        Task<EnquiryDto> AddAsync(Enquiry enquiry);
        Task<bool> EditAsync(int id, Enquiry enquiry);
        Task<bool> DeleteAsync(int id);
        Task<User> GetRandomNonAdminUserAsync();
    }
}
