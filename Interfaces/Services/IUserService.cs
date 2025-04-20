using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;

namespace VNFarm_FinalFinal.Interfaces.Services
{
    public interface IUserService : IService<User, UserRequestDTO, UserResponseDTO>
    {
        Task<bool> SetUserActiveAsync(int userId, bool isActive);
        Task<UserResponseDTO?> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email);
    }
} 