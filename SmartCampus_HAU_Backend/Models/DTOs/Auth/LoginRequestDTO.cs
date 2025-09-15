using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.DTOs.Auth
{
    public class LoginRequestDTO
    {
        [Required]
        public string EmailOrUsername { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
