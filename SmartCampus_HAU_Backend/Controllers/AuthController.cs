using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SmartCampus_HAU_Backend.Models.DTOs.Auth;
using SmartCampus_HAU_Backend.Models.DTOs.Users;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Services.Interfaces;

namespace SmartCampus_HAU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService,
            IUserService userService)
                {
                    _userManager = userManager;
                    _signInManager = signInManager;
                    _tokenService = tokenService;
                    _userService = userService;
                }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.EmailOrUsername) ?? await _userManager.FindByNameAsync(request.EmailOrUsername);
                if (user == null)
                {
                    return BadRequest(new { message = "Không tìm thấy người dùng" });
                }

                var Email = user.Email;
                if (!user.EmailConfirmed)
                {
                    await _userService.ResendEmailConfirmationAsync(Email);
                    return new UnauthorizedObjectResult(new
                    {
                        Message = "Email chưa được xác nhận. Vui lòng kiểm tra email để xác nhận tài khoản.",
                        Email
                    });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác" });
                }

                var tokenResponse = await _tokenService.GenerateTokenAsync(user);

                var response = new LoginResponseDTO
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    Token = tokenResponse
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đăng nhập thất bại", error = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new { message = "Đăng xuất thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đăng xuất thất bại", error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            try
            {
                var tokenResponse = await _tokenService.RefreshTokenAsync(request.RefreshToken);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Token refresh failed", error = ex.Message });
            }
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDTO request)
        {
            try
            {
                var success = await _tokenService.RevokeTokenAsync(request.RefreshToken);
                if (success)
                {
                    return Ok(new { message = "Token revoked successfully" });
                }
                return BadRequest(new { message = "Token revocation failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Token revocation failed", error = ex.Message });
            }
        }
    }
}
