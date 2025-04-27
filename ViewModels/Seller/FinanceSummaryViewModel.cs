using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Seller
{
    public class FinanceSummaryViewModel
    {
        public decimal TotalRevenueMonth { get; set; }
        public int TotalOrdersMonth { get; set; }
        public decimal TotalRevenueAllTime { get; set; }
        public int TotalOrdersAllTime { get; set; }
        public List<int> MonthlyRevenue { get; set; } = new();
        public List<PaymentMethodResponseDTO> PaymentMethodsAvailable { get; set; } = new();
        public List<TransactionResponseDTO> RecentTransactions { get; set; } = new();
    }
}
