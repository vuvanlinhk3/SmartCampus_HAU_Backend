using SmartCampus_HAU_Backend.Models.DTOs.Auth;
using SmartCampus_HAU_Backend.Models.Entities;
using System.Security.Claims;

namespace SmartCampus_HAU_Backend.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokenAsync(User user);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
