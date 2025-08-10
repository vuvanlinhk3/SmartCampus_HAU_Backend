using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Users;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Data;
using Microsoft.AspNetCore.Identity;

namespace SmartCampus_HAU_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<RegisterDTO> RegisterNewUserAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null || string.IsNullOrEmpty(registerDTO.UserName) || string.IsNullOrEmpty(registerDTO.Email) || string.IsNullOrEmpty(registerDTO.Password))
            {
                throw new ArgumentException("Invalid registration data.");
            }

            var user = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
            };

            var createNewUser = await _userManager.CreateAsync(user, registerDTO.Password);

            return registerDTO;
        }
    }
}
