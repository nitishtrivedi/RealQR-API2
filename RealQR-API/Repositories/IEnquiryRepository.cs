using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public interface IEnquiryRepository
    {
        Task<IEnumerable<Enquiry>> GetAllAsync();
        Task<Enquiry> GetAsync(int id);
        Task<Enquiry> AddAsync(Enquiry enquiry);
        Task<bool> EditAsync(int id, Enquiry enquiry);
        Task<bool> DeleteAsync(int id);
    }
}
