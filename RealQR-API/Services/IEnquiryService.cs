using RealQR_API.DTOs;
using RealQR_API.Models;

namespace RealQR_API.Services
{
    public interface IEnquiryService
    {
        Task<IEnumerable<EnquiryDto>> GetEnquiriesAsync();
        Task<EnquiryDto> GetEnquiryAsync(int id);
        Task<EnquiryDto> AddEnquiryAsync(Enquiry enquiry);
        Task<bool> EditEnquiryAsync(int id, Enquiry enquiry);
        Task<bool> DeleteEnquiryAsync(int id);
    }
}
