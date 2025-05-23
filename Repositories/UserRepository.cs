using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Repositories.Interfaces;

namespace VNFarm.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(VNFarmContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.Include(u => u.Store)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }
        
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbSet
                .AnyAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
} 