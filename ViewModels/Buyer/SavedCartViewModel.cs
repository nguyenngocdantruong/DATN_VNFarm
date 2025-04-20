using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.ViewModels.Buyer
{
    public class SavedCartViewModel
    {
        public List<OrderResponseDTO> OrderCarts { get; set; } = new();
        public Dictionary<SortType, string> SortTypes { get; set; } = new();
    }
}
