using System.Security.Claims;

namespace SmartCampus_HAU_Backend.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
