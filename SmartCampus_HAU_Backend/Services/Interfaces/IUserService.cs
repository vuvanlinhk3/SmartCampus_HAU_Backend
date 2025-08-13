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
    }
}
