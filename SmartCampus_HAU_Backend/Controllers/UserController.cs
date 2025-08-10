using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace SmartCampus_HAU_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return BadRequest("Invalid registration data.");
            }
            try
            {
                var result = await _userService.RegisterNewUserAsync(registerDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Fix: StatusCode is now accessible
            }
        }
    }
}
