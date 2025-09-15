using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.DTOs.Auth
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
