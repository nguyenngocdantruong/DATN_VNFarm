namespace VNFarm.Entities
{
    public class ShopCart : BaseEntity
    {
        public int ShopId { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        public Store? Shop { get; set; }
        public List<CartItem> CartItems { get; set; } = [];
    }
}
