using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;

namespace VNFarm.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(VNFarmContext context) : base(context)
        {
        }
    }
} 