namespace VNFarm.Entities
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<ShopCart> ShopCarts { get; set; } = [];
    }
}
