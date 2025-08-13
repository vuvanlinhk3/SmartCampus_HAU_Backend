using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.DTOs.Users
{
    public class RegisterDTO
    {
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }
    }
}
