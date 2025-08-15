namespace SmartCampus_HAU_Backend.Models.DTOs.Users
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
