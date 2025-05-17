using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;

namespace VNFarm.Repositories
{
    public class ContactRequestRepository : BaseRepository<ContactRequest>, IContactRequestRepository
    {
        public ContactRequestRepository(VNFarmContext context) : base(context)
        {
        }
    }
} 