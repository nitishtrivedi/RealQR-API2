using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealQR_API.Models;
using RealQR_API.Services;

namespace RealQR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Only admin users can access the list of users." });
            }
            var users = await _userService.GetUsers();
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            //COMMENTED BECAUSE USERS CAN EDIT THEIR OWN PROFILES IF NEED BE

            //if (!User.IsInRole("Admin"))
            //{
            //    return Unauthorized(new { Message = "Only admin users can access a user" });
            //}
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound(new {Message = $"User with ID: {id} not found."});
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Only admin users can add a user" });
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newUser = await _userService.AddUser(user);
            return CreatedAtAction(nameof(GetUserById), new {id = user.Id}, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromBody] User user, int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Only admin users can edit a user." });
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != user.Id) return BadRequest(new { Message = "User ID mismatch" });
            var success = await _userService.EditUser(user, id);
            if (!success) return NotFound(new {Message = $"User with ID: {id} not found."});
            return Ok(new {Message = $"User with ID: {id} modified successfully."});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized(new { Message = "Only admin users can delete a user." });
            }
            var success = await _userService.DeleteUser(id);
            if (!success) return BadRequest(new {Message = $"user with ID: {id} not found."});
            return Ok(new { Message = "User Deleted successfully, assigned enquiries set to unassigned" });
        }
    }
}
