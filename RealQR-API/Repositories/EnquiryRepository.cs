using Microsoft.EntityFrameworkCore;
using RealQR_API.DBContext;
using RealQR_API.DTOs;
using RealQR_API.Models;

namespace RealQR_API.Repositories
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly RealQRDBContext _dbContext;
        public EnquiryRepository(RealQRDBContext dbContext) => _dbContext = dbContext;


        public async Task<IEnumerable<EnquiryDto>> GetAllAsync()
        {
            var enquiries = await _dbContext.Enquiry.Include(e => e.EnquiryQuestionnaire).Include(e => e.User).ToListAsync();
            return enquiries.Select(e => MapToDto(e)).ToList();
        }

        public async Task<EnquiryDto> GetAsync(int id)
        {
            var enquiry = await _dbContext.Enquiry
                .Include(e => e.EnquiryQuestionnaire)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (enquiry == null) throw new KeyNotFoundException($"Enquiry with ID: {id} not found");
            return MapToDto(enquiry);
        }

        public async Task<EnquiryDto> AddAsync(Enquiry enquiry)
        {
            if (enquiry == null) throw new ArgumentNullException(nameof(enquiry));

            _dbContext.Enquiry.Add(enquiry);
            await _dbContext.SaveChangesAsync();

            var questionnaire = new EnquiryQuestionnaire
            {
                EnquiryId = enquiry.Id,
                EnquiryStatus = "Open",
                AgentName = ""
            };
            _dbContext.EnquiryQuestionnaire.Add(questionnaire);
            await _dbContext.SaveChangesAsync();
            return MapToDto(enquiry);
        }

        public async Task<bool> EditAsync(int id, Enquiry enquiry)
        {
            if (id != enquiry.Id) return false;
            var existingEnquiry = await _dbContext.Enquiry.FindAsync(id);
            if (existingEnquiry == null) return false;
            _dbContext.Entry(existingEnquiry).CurrentValues.SetValues(enquiry);

            if (existingEnquiry.EnquiryQuestionnaire == null)
            {
                existingEnquiry.EnquiryQuestionnaire = new EnquiryQuestionnaire
                {
                    EnquiryId = enquiry.Id,
                    EnquiryStatus = "Open",
                    AgentName = enquiry.User != null ? $"{enquiry.User.FirstName} {enquiry.User.LastName}" : ""
                };
                _dbContext.EnquiryQuestionnaire.Add(existingEnquiry.EnquiryQuestionnaire);
            }
            else if(enquiry.EnquiryQuestionnaire != null)
            {
                _dbContext.Entry(existingEnquiry.EnquiryQuestionnaire).CurrentValues.SetValues(enquiry.EnquiryQuestionnaire);
                existingEnquiry.EnquiryQuestionnaire.AgentName = enquiry.User != null ? $"{enquiry.User.FirstName} {enquiry.User.LastName}" : "";
            }

            var result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var enquiry = await _dbContext.Enquiry.FindAsync(id);
            if (enquiry == null) return false;

            if(enquiry.EnquiryQuestionnaire != null)
            {
                _dbContext.EnquiryQuestionnaire.Remove(enquiry.EnquiryQuestionnaire);
            }
            _dbContext.Enquiry.Remove(enquiry);
            var result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<User> GetRandomNonAdminUserAsync()
        {
            var nonAdminUsers = await _dbContext.Users
                .Where(u => !u.IsUserAdmin)
                .ToListAsync();
            if (!nonAdminUsers.Any()) return null;
            var random = new Random();
            return nonAdminUsers[random.Next(nonAdminUsers.Count)];
        }

        private EnquiryDto MapToDto(Enquiry enquiry)
        {
            return new EnquiryDto
            {
                Id = enquiry.Id,
                FirstName = enquiry.FirstName,
                LastName = enquiry.LastName,
                ContactNumber = enquiry.ContactNumber,
                Email = enquiry.Email,
                MethodOfContact = enquiry.MethodOfContact,
                BudgetRange = enquiry.BudgetRange,
                PreferredAreas = enquiry.PreferredAreas,
                PropertyType = enquiry.PropertyType,
                ModeOfPayment = enquiry.ModeOfPayment,
                PurchaseTimeFrame = enquiry.PurchaseTimeFrame,
                PurchaseType = enquiry.PurchaseType,
                OtherQuestions = enquiry.OtherQuestions,
                ConsentToCall = enquiry.ConsentToCall,
                UserId = enquiry.UserId,
                User = enquiry.User != null ? new UserDto
                {
                    Id = enquiry.User.Id,
                    UserName = enquiry.User.UserName,
                    FirstName = enquiry.User.FirstName,
                    LastName = enquiry.User.LastName,
                    Email = enquiry.User.Email,
                    IsUserAdmin = enquiry.User.IsUserAdmin
                } : null,
                EnquiryQuestionnaire = enquiry.EnquiryQuestionnaire != null ? new EnquiryQuestionnaireDto
                {
                    Id = enquiry.EnquiryQuestionnaire.Id,
                    EnquiryId = enquiry.EnquiryQuestionnaire.EnquiryId,
                    EnquiryStatus = enquiry.EnquiryQuestionnaire.EnquiryStatus,
                    AgentName = enquiry.EnquiryQuestionnaire.AgentName,
                    HasConfirmedBudget = enquiry.EnquiryQuestionnaire.HasConfirmedBudget,
                    RefinedBudgetRange = enquiry.EnquiryQuestionnaire.RefinedBudgetRange,
                    ReconfirmModeOfPayment = enquiry.EnquiryQuestionnaire.ReconfirmModeOfPayment,
                    LoanProcessingConsent = enquiry.EnquiryQuestionnaire.LoanProcessingConsent,
                    LoanProcessingVendor = enquiry.EnquiryQuestionnaire.LoanProcessingVendor,
                    FollowUpActions = enquiry.EnquiryQuestionnaire.FollowUpActions,
                    Comments = enquiry.EnquiryQuestionnaire.Comments
                } : null
            };
        }
    }
}
