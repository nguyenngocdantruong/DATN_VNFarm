using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Admin
{
    public class AdminSummaryViewModel
    {
        public required int TotalOrders { get; set; }
        public required int TotalUsers { get; set; }
        public required int TotalProducts { get; set; }
        public required decimal TotalRevenue { get; set; }
        public required List<ProductResponseDTO?> ProductDTOs { get; set; } = new();
        public required int TotalProductPages { get; set; }
        public required int ProductPageSize { get; set; }
        public required List<OrderResponseDTO?> OrderDTOs { get; set; } = new();
        public required int TotalOrderPages { get; set; }
        public required int OrderPageSize { get; set; }
    }
}


