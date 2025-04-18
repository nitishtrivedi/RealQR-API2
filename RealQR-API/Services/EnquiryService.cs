﻿using Microsoft.EntityFrameworkCore;
using RealQR_API.DBContext;
using RealQR_API.Models;
using RealQR_API.Repositories;

namespace RealQR_API.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository _enquiryRepository;
        public EnquiryService(IEnquiryRepository enquiryRepository) => _enquiryRepository = enquiryRepository;

        public async Task<IEnumerable<Enquiry>> GetEnquiriesAsync() => await _enquiryRepository.GetAllAsync();

        public async Task<Enquiry> GetEnquiryAsync(int id) => await _enquiryRepository.GetAsync(id);

        public async Task<Enquiry> AddEnquiryAsync(Enquiry enquiry) => await _enquiryRepository.AddAsync(enquiry);

        public async Task<bool> EditEnquiryAsync(int id, Enquiry enquiry) => await _enquiryRepository.EditAsync(id, enquiry);

        public async Task<bool> DeleteEnquiryAsync(int id) => await _enquiryRepository.DeleteAsync(id);
    }
}
