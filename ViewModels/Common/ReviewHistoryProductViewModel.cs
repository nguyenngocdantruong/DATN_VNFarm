using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Common
{
    public class ReviewHistoryProductViewModel
    {
        public List<ReviewResponseDTO> Reviews { get; set; } = new();
    }
}
