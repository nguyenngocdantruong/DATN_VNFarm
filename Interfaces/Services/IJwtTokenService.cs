
using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.Interfaces.Services
{
    public interface IJwtTokenService
    {
        TokenResultDTO GenerateToken(int userId, string email, string role);
    }
}
