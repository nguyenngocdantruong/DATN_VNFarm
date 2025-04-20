using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm.Infrastructure.Persistence.Context;

namespace VNFarm.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(VNFarmContext context) : base(context)
        {
        }
    }
} 