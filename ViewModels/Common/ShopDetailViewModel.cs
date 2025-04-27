using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Common
{
    public class ShopDetailViewModel
    {
        public StoreResponseDTO? Store { get; set; } 
        public int TotalProducts { get; set; }
        public int TotalSoldOutProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<ProductResponseDTO> Products { get; set; } = new();
        public ProductCriteriaFilter ProductCriteriaFilter { get; set; } = new();
    }
}
