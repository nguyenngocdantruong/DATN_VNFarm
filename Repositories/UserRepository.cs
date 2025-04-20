using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm.Infrastructure.Persistence.Context;

namespace VNFarm.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(VNFarmContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }
        
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbSet
                .AnyAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
} 