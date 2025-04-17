using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealQR_API.Models;
using RealQR_API.Services;

namespace RealQR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;
        public EnquiryController(IEnquiryService enquiryService) => _enquiryService = enquiryService;

        [HttpGet]
        public async Task<IActionResult> GetEnquiries() => Ok(await _enquiryService.GetEnquiriesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnquiry(int id)
        {
            var enquiry = await _enquiryService.GetEnquiryAsync(id);
            if (enquiry == null) return NotFound(new {Message = $"Enquiry with ID: {id} Not Found"});
            return Ok(enquiry);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddEnquiry([FromBody] Enquiry enquiry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newEnquiry = await _enquiryService.AddEnquiryAsync(enquiry);
            return CreatedAtAction(nameof(GetEnquiry), new { id = newEnquiry.Id }, newEnquiry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditEnquiry(int id, [FromBody] Enquiry enquiry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _enquiryService.EditEnquiryAsync(id, enquiry);
            if (!success) return NotFound(new { Message = $"Enquiry with ID: {id} not found" });
            return Ok(new {Message = $"Enquiry with ID: {id} edited successfully"});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            var success = await _enquiryService?.DeleteEnquiryAsync(id);
            if (!success) return BadRequest(new { Message = $"Enquiry with ID: {id} not found." });
            return Ok(new {Message = "Enquiry deleted successfully"});
        }


    }
}
