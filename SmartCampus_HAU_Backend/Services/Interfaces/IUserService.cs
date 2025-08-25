using Microsoft.AspNetCore.Mvc;
using SmartCampus_HAU_Backend.Models.DTOs.Users;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegisterNewUserAsync(RegisterDTO registerDTO); // Đăng ký người dùng mới
        Task<IActionResult> LoginAsync(LoginDTO loginDTO); // Đăng nhập
        Task<IActionResult> ConfirmEmailAsync(string userId, string token); // Xác nhận email
        Task<IActionResult> ResendEmailConfirmationAsync(string email); // Gửi lại email xác nhận
        Task<IActionResult> SendForgotPasswordEmail(string email); // Gửi email quên mật khẩu
        Task<IActionResult> ResetPasswordAsync(string email, string token, string newPassword); // Đặt lại mật khẩu
        Task<IActionResult> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordDTO); // Đổi mật khẩu
        Task<IActionResult> UpdateUserInfoAsync(string userId, UpdateUserInfoDTO updateUserInfoDTO); // Cập nhật thông tin người dùng
        Task<IActionResult> GetUserInfoAsync(string userId); // Lấy thông tin người dùng
        Task<IActionResult> DeleteUsersAsync(string username); // Xóa người dùng
    }
}
