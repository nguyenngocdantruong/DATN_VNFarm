using System.Security.Claims;
using VNFarm.DTOs.Response;

namespace VNFarm.Services.Interfaces
{
    public interface IJwtTokenService
    {
        TokenResultDTO GenerateToken(int userId, string email, string role);
        int? GetUserIdFromToken(string token);
        string? GetRoleFromToken(string token);
        int? GetUserIdFromToken(ClaimsPrincipal claimsPrincipal);
    }
}
