using RealQR_API.Models;

namespace RealQR_API.Services
{
    public interface IEnquiryService
    {
        Task<List<Enquiry>> GetEnquiriesAsync();
        Task<Enquiry> GetEnquiryAsync(int id);
        Task<Enquiry> AddEnquiryAsync(Enquiry enquiry);
        Task<bool> EditEnquiryAsync(int id, Enquiry enquiry);
        Task<bool> DeleteEnquiryAsync(int id);
    }
}
