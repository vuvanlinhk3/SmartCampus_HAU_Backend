using System.ComponentModel.DataAnnotations;

namespace SmartCampus_HAU_Backend.Models.DTOs.Users
{
    public class UpdateUserInfoDTO
    {
        [StringLength(255)]
        public string? FullName { get; set; }

        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
    }
}
