using Microsoft.EntityFrameworkCore;
using VNFarm.Infrastructure.Persistence.Context;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Interfaces.Repositories;

namespace VNFarm.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(VNFarmContext context) : base(context)
        {
        }

        public async Task<PaymentMethod> AddPaymentMethodAsync(PaymentMethod paymentMethod)
        {
            await _context.PaymentMethods.AddAsync(paymentMethod);
            await _context.SaveChangesAsync();
            return paymentMethod;
        }

        public async Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id)
        {
            return await _context.PaymentMethods.FindAsync(id);
        }

        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(int storeId, DateTime startDate, DateTime endDate)
        {
            var totalRevenue = await _context.Transactions
                .Where(t => t.Order != null && t.Order.StoreId == storeId && t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .SumAsync(t => t.Amount);
            return totalRevenue;
        }

        public async Task<Transaction?> GetTransactionByOrderIdAsync(int orderId)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.OrderId == orderId);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByStoreIdAsync(int storeId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Order)
                .Where(t => t.Order != null && t.Order.StoreId == storeId)
                .ToListAsync();
            return transactions;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Order)
                .Where(t => t.Order != null && t.Order.BuyerId == userId)
                .ToListAsync();
            return transactions;
        }

        public async Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus status)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
            if (transaction != null)
            {
                transaction.Status = status;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Transaction not found");
            }
        }
    }
}