namespace SmartCampus_HAU_Backend.Models.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public TokenResponse Token { get; set; } = null!;
    }
}
