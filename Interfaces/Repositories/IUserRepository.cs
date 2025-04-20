using VNFarm_FinalFinal.Entities;

namespace VNFarm_FinalFinal.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email);
    }
} 