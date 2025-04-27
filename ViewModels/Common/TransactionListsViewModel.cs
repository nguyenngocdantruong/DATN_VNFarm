using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Common
{
    public class TransactionListsViewModel
    {
        public List<TransactionResponseDTO>? Transactions { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
