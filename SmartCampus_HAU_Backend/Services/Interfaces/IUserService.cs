using SmartCampus_HAU_Backend.Models.DTOs.Users;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterDTO> RegisterNewUserAsync(RegisterDTO registerDTO);
    }
}
