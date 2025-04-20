using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.ViewModels.Common
{
    public class TransactionListsViewModel
    {
        public List<TransactionResponseDTO>? Transactions { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
