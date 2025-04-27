using VNFarm.DTOs.Response;
using VNFarm.Enums;

namespace VNFarm.ViewModels.Buyer
{
    public class SavedCartViewModel
    {
        public List<OrderResponseDTO> OrderCarts { get; set; } = new();
        public Dictionary<SortType, string> SortTypes { get; set; } = new();
    }
}
