using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {

        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId);
        Task<IEnumerable<Transaction>> GetTransactionsByStoreIdAsync(int storeId);
        Task<Transaction?> GetTransactionByOrderIdAsync(int orderId);
        Task<decimal> GetTotalRevenueAsync(int storeId, DateTime startDate, DateTime endDate);
        Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus status);
        #region Payment Method
        Task<PaymentMethod> AddPaymentMethodAsync(PaymentMethod paymentMethod);
        Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync();
        Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id);
        #endregion
    }
} 