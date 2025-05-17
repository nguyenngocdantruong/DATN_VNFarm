using Microsoft.EntityFrameworkCore;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;

namespace VNFarm.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(VNFarmContext context) : base(context)
        {
        }

        
    }
}
