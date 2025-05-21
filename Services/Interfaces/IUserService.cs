using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Services.Interfaces
{
    public interface IUserService : IService<User, UserRequestDTO, UserResponseDTO>
    {
        Task<bool> SetUserActiveAsync(int userId, bool isActive);
        Task<UserResponseDTO?> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email);
    }
} 