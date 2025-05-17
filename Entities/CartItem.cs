namespace VNFarm.Entities
{
    public class CartItem : BaseEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ShopCartId { get; set; }
        public Product? Product { get; set; }
    }
}
