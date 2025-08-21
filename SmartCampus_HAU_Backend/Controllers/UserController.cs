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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            return await _userService.ConfirmEmailAsync(userId, token);
        }

        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] string email)
        {
            return await _userService.ResendEmailConfirmationAsync(email);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _userService.LoginAsync(loginDTO);

            if (result is OkObjectResult okResult && okResult.Value != null)
            {
                try
                {
                    var resultValue = okResult.Value;
                    var resultType = resultValue.GetType();

                    var userIdProperty = resultType.GetProperty("UserId");
                    if (userIdProperty != null)
                    {
                        var userId = userIdProperty.GetValue(resultValue)?.ToString();
                        if (!string.IsNullOrEmpty(userId))
                        {
                            HttpContext.Session.SetString("UserId", userId);
                            HttpContext.Session.SetString("UserName", loginDTO.UserName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save session: {ex.Message}");
                }
            }
            return result;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            return await _userService.SendForgotPasswordEmail(email);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetDTO)
        {
            if (resetDTO == null || string.IsNullOrEmpty(resetDTO.Email) ||
                string.IsNullOrEmpty(resetDTO.Token) || string.IsNullOrEmpty(resetDTO.NewPassword))
            {
                return BadRequest("Thông tin đặt lại mật khẩu không hợp lệ");
            }

            try
            {
                return await _userService.ResetPasswordAsync(resetDTO.Email, resetDTO.Token, resetDTO.NewPassword);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Vui lòng đăng nhập để sử dụng chức năng này");
            }

            return await _userService.ChangePasswordAsync(userId, changePasswordDTO);
        }

        [HttpPut("update-info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoDTO updateUserInfoDTO)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Vui lòng đăng nhập để sử dụng chức năng này");
            }

            return await _userService.UpdateUserInfoAsync(userId, updateUserInfoDTO);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Vui lòng đăng nhập để sử dụng chức năng này");
            }

            return await _userService.GetUserInfoAsync(userId);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Tên người dùng không được để trống");
            }
            return await _userService.DeleteUsersAsync(username);
        }
    }
}
