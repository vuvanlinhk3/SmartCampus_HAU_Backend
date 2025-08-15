namespace SmartCampus_HAU_Backend.Models.DTOs.Users
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
