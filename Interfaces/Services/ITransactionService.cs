using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Services
{
    public interface ITransactionService : IService<Transaction, TransactionRequestDTO, TransactionResponseDTO>
    {
        Task<IEnumerable<TransactionResponseDTO?>> GetTransactionsByUserIdAsync(int userId);
        Task<IEnumerable<TransactionResponseDTO?>> GetTransactionsByStoreIdAsync(int storeId);
        Task<TransactionResponseDTO?> GetTransactionByOrderIdAsync(int orderId);
        Task<decimal> GetTotalRevenueAsync(int storeId, DateTime startDate, DateTime endDate);
        Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus status);
        #region Payment Method
        Task<PaymentMethodResponseDTO?> AddPaymentMethodAsync(int userId, PaymentMethodRequestDTO paymentMethodRequestDTO);
        Task<IEnumerable<PaymentMethodResponseDTO?>> GetPaymentMethodsByUserIdAsync(int userId);
        Task<IEnumerable<PaymentMethodResponseDTO?>> GetPaymentMethodsAsync();
        Task<PaymentMethodResponseDTO?> GetPaymentMethodByIdAsync(int id);
        Task<PaymentMethodResponseDTO?> GetPaymentMethodByOrderIdAsync(int orderId);
        Task<decimal> CalculateTotalRevenueByStoreIdAsync(int storeId);
        #endregion
    }
} 