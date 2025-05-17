using VNFarm.DTOs.Response;

namespace VNFarm.Interfaces.Services
{
    public interface IJwtTokenService
    {
        TokenResultDTO GenerateToken(int userId, string email, string role);
        int? GetUserIdFromToken(string token);
        string? GetRoleFromToken(string token);
    }
}
