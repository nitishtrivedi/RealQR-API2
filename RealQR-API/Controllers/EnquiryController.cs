using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealQR_API.DBContext;
using RealQR_API.DTOs;
using RealQR_API.Models;
using RealQR_API.Repositories;
using RealQR_API.Services;
using System.Security.Claims;

namespace RealQR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;
        private readonly RealQRDBContext _dbContext;
        private readonly IEnquiryRepository _enquiryRepository;
        public EnquiryController(IEnquiryService enquiryService, RealQRDBContext dbContext, IEnquiryRepository enquiryRepository)
        {
            _enquiryService = enquiryService;
            _dbContext = dbContext;
            _enquiryRepository = enquiryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnquiryDto>>> GetEnquiries() => Ok(await _enquiryService.GetEnquiriesAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<EnquiryDto>> GetEnquiry(int id)
        {
            var enquiry = await _enquiryService.GetEnquiryAsync(id);
            if (enquiry == null) return NotFound(new {Message = $"Enquiry with ID: {id} Not Found"});
            return Ok(enquiry);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<EnquiryDto>> AddEnquiry([FromBody] EnquiryDto enquiryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var enquiry = MapToEntity(enquiryDto);
            var newEnquiry = await _enquiryService.AddEnquiryAsync(enquiry);
            return CreatedAtAction(nameof(GetEnquiry), new { id = newEnquiry.Id }, newEnquiry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditEnquiry(int id, [FromBody] EnquiryDto enquiryDto)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            //var enquiry = MapToEntity(enquiryDto);
            //var success = await _enquiryService.EditEnquiryAsync(id, enquiry);
            //if (!success) return NotFound(new { Message = $"Enquiry with ID: {id} not found" });
            //return Ok(new {Message = $"Enquiry with ID: {id} edited successfully"});

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != enquiryDto.Id) return BadRequest(new { Message = $"Enquiry ID Mismatch" });

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var currentUser = await _dbContext.Users.FindAsync(currentUserId);

            if (currentUser == null) return Unauthorized(new { Message = "Current user not found" });

            var enquiry = MapToEntity(enquiryDto);

            if (currentUser.IsUserAdmin)
            {
                if (enquiryDto.UserId == null || enquiryDto.UserId == 0)
                {
                    // Admin chooses not to assign; set to unassigned or assign randomly
                    var existingEnquiry = await _dbContext.Enquiry.FindAsync(id);
                    if (existingEnquiry != null && existingEnquiry.UserId == null)
                    {
                        var nonAdminUser = await _enquiryRepository.GetRandomNonAdminUserAsync();
                        if (nonAdminUser != null)
                        {
                            enquiry.UserId = nonAdminUser.Id;
                            enquiry.User = nonAdminUser;
                        }
                    }
                }
                else
                {
                    // Admin specifies a user ID
                    var assignedUser = await _dbContext.Users.FindAsync(enquiryDto.UserId);
                    if (assignedUser == null) return BadRequest(new { Message = $"User with ID: {enquiryDto.UserId} not found" });
                    enquiry.UserId = assignedUser.Id;
                    enquiry.User = assignedUser;
                }
            }
            else
            {
                // Non-admin users can only edit enquiries assigned to them or unassigned
                var existingEnquiry = await _dbContext.Enquiry.FindAsync(id);
                if (existingEnquiry == null || (existingEnquiry.UserId != currentUserId && existingEnquiry.UserId != null))
                {
                    return Unauthorized(new { Message = "You are not authorized to edit this enquiry" });
                }
                enquiry.UserId = currentUserId;
                enquiry.User = currentUser;
            }

            var success = await _enquiryService.EditEnquiryAsync(id, enquiry);
            if (!success) return NotFound(new { Message = $"Enquiry with ID: {id} not found" });
            return Ok(new { Message = $"Enquiry with ID: {id} edited successfully" });


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            var success = await _enquiryService?.DeleteEnquiryAsync(id);
            if (!success) return BadRequest(new { Message = $"Enquiry with ID: {id} not found." });
            return Ok(new {Message = "Enquiry deleted successfully"});
        }

        //Questionnaire Edit Method
        [HttpPut("{id}/questionnaire")]
        public async Task<IActionResult> UpdateQuestionnaire(int id, [FromBody] EnquiryQuestionnaireDto questionnaireDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != questionnaireDto.EnquiryId) return BadRequest(new {Message = "Enquiry ID Mismatch"});
            try
            {
                var enquiryDto = await _enquiryService.GetEnquiryAsync(id);
                if (enquiryDto == null) return NotFound(new { Message = $"Enquiry with ID: {id} not found" });
                var enquiry = MapToEntity(enquiryDto);
                var questionnaire = MapToEntity(enquiry, questionnaireDto);

                var existingQuestionnaire = await _dbContext.EnquiryQuestionnaire.FindAsync(questionnaire.Id);
                if (existingQuestionnaire == null)
                {
                    _dbContext.EnquiryQuestionnaire.Add(questionnaire);
                }
                else
                {
                    _dbContext.Entry(existingQuestionnaire).State = EntityState.Detached;
                    _dbContext.Entry(existingQuestionnaire).State = EntityState.Modified;
                    _dbContext.Entry(existingQuestionnaire).CurrentValues.SetValues(questionnaire);

                }
                await _dbContext.SaveChangesAsync();
                return Ok(new { Message = "Questionnaire updated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new {Message = ex.Message});
            }
        }

        private Enquiry MapToEntity(EnquiryDto dto)
        {
            return new Enquiry
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                MethodOfContact = dto.MethodOfContact,
                BudgetRange = dto.BudgetRange,
                PreferredAreas = dto.PreferredAreas,
                PropertyType = dto.PropertyType,
                ModeOfPayment = dto.ModeOfPayment,
                PurchaseTimeFrame = dto.PurchaseTimeFrame,
                PurchaseType = dto.PurchaseType,
                OtherQuestions = dto.OtherQuestions,
                ConsentToCall = dto.ConsentToCall,
                UserId = dto.UserId,
                EnquiryQuestionnaire = dto.EnquiryQuestionnaire != null ? new EnquiryQuestionnaire
                {
                    Id = dto.EnquiryQuestionnaire.Id,
                    EnquiryId = dto.EnquiryQuestionnaire.EnquiryId,
                    EnquiryStatus = dto.EnquiryQuestionnaire.EnquiryStatus,
                    AgentName = dto.EnquiryQuestionnaire.AgentName,
                    HasConfirmedBudget = dto.EnquiryQuestionnaire.HasConfirmedBudget,
                    RefinedBudgetRange = dto.EnquiryQuestionnaire.RefinedBudgetRange,
                    ReconfirmModeOfPayment = dto.EnquiryQuestionnaire.ReconfirmModeOfPayment,
                    LoanProcessingConsent = dto.EnquiryQuestionnaire.LoanProcessingConsent,
                    LoanProcessingVendor = dto.EnquiryQuestionnaire.LoanProcessingVendor,
                    FollowUpActions = dto.EnquiryQuestionnaire.FollowUpActions,
                    Comments = dto.EnquiryQuestionnaire.Comments
                } : null
            };
        }

        private EnquiryQuestionnaire MapToEntity(Enquiry enquiry, EnquiryQuestionnaireDto dto)
        {
            return new EnquiryQuestionnaire
            {
                Id = dto.Id,
                EnquiryId = enquiry.Id,
                EnquiryStatus = dto.EnquiryStatus,
                AgentName = dto.AgentName,
                HasConfirmedBudget = dto.HasConfirmedBudget,
                RefinedBudgetRange = dto.RefinedBudgetRange,
                ReconfirmModeOfPayment = dto.ReconfirmModeOfPayment,
                LoanProcessingConsent = dto.LoanProcessingConsent,
                LoanProcessingVendor = dto.LoanProcessingVendor,
                FollowUpActions = dto.FollowUpActions,
                Comments = dto.Comments
            };
        }
    }
}
