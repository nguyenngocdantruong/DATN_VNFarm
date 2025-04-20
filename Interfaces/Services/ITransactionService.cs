using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Services
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
        Task<IEnumerable<PaymentMethodResponseDTO?>> GetPaymentMethodsAsync();
        Task<PaymentMethodResponseDTO?> GetPaymentMethodByIdAsync(int id);
        Task<PaymentMethodResponseDTO?> GetPaymentMethodByOrderIdAsync(int orderId);
        #endregion
    }
} 