using Microsoft.AspNetCore.Mvc;
using SmartCampus_HAU_Backend.Models.DTOs.Users;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegisterNewUserAsync(RegisterDTO registerDTO);
        Task<IActionResult> LoginAsync(LoginDTO loginDTO);
        Task<IActionResult> ConfirmEmailAsync(string userId, string token);
        Task<IActionResult> ResendEmailConfirmationAsync(string email);
        Task<IActionResult> SendForgotPasswordEmail(string email);
        Task<IActionResult> ResetPasswordAsync(string email, string token, string newPassword);
        Task<IActionResult> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordDTO);
        Task<IActionResult> UpdateUserInfoAsync(string userId, UpdateUserInfoDTO updateUserInfoDTO);
        Task<IActionResult> GetUserInfoAsync(string userId);
        Task<IActionResult> DeleteUsersAsync(string username);
    }
}
