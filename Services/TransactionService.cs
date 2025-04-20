using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Helpers;
using VNFarm_FinalFinal.Mappers;

namespace VNFarm.Infrastructure.Services
{
    public class TransactionService : BaseService<Transaction, TransactionRequestDTO, TransactionResponseDTO>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionService> _logger;
        private readonly IOrderService _orderService;
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        public TransactionService(
            ITransactionRepository transactionRepository,
            IOrderService orderService,
            IRepository<PaymentMethod> paymentMethodRepository,
            ILogger<TransactionService> logger) : base(transactionRepository)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
            _orderService = orderService;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<IEnumerable<TransactionResponseDTO?>> GetTransactionsByUserIdAsync(int userId)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId);
                return transactions.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách giao dịch của người dùng ID: {userId}");
                return new List<TransactionResponseDTO>();
            }
        }

        public async Task<IEnumerable<TransactionResponseDTO?>> GetTransactionsByStoreIdAsync(int storeId)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsByStoreIdAsync(storeId);
                return transactions.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách giao dịch của cửa hàng ID: {storeId}");
                return new List<TransactionResponseDTO>();
            }
        }

        public async Task<TransactionResponseDTO?> GetTransactionByOrderIdAsync(int orderId)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionByOrderIdAsync(orderId);
                return MapToDTO(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin giao dịch của đơn hàng ID: {orderId}");
                return null;
            }
        }

        public async Task<decimal> GetTotalRevenueAsync(int storeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _transactionRepository.GetTotalRevenueAsync(storeId, startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tính tổng doanh thu của cửa hàng ID: {storeId} từ {startDate} đến {endDate}");
                return 0;
            }
        }

        public async Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus status)
        {
            try
            {
                await _transactionRepository.UpdateTransactionStatusAsync(transactionId, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật trạng thái giao dịch ID: {transactionId} thành {status}");
                throw;
            }
        }

        public async override Task<IQueryable<Transaction>> Query(IFilterCriteria filter)
        {
            var query = await _transactionRepository.GetQueryableAsync();
            if (filter is TransactionCriteriaFilter transactionCriteriaFilter)
            {
                // Áp dụng bộ lọc
                if (!string.IsNullOrEmpty(transactionCriteriaFilter.SearchTerm))
                {
                    query = query.Where(t =>
                        t.TransactionCode.Contains(transactionCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        t.Details.Contains(transactionCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                // Lọc theo khoảng tiền
                if (transactionCriteriaFilter.MinAmount.HasValue)
                {
                    query = query.Where(t => t.Amount >= transactionCriteriaFilter.MinAmount.Value);
                }

                if (transactionCriteriaFilter.MaxAmount.HasValue && transactionCriteriaFilter.MaxAmount.Value < decimal.MaxValue)
                {
                    query = query.Where(t => t.Amount <= transactionCriteriaFilter.MaxAmount.Value);
                }

                // Lọc theo phương thức thanh toán
                if (transactionCriteriaFilter.PaymentMethod != PaymentMethodEnum.All)
                {
                    query = query.Where(t => t.PaymentMethod == transactionCriteriaFilter.PaymentMethod);
                }

                // Lọc theo trạng thái giao dịch
                if (transactionCriteriaFilter.Status != TransactionStatus.All)
                {
                    query = query.Where(t => t.Status == transactionCriteriaFilter.Status);
                }

                // Lọc theo khoảng thời gian
                if (transactionCriteriaFilter.StartDate.HasValue)
                {
                    query = query.Where(t => t.CreatedAt >= transactionCriteriaFilter.StartDate.Value);
                }

                if (transactionCriteriaFilter.EndDate.HasValue)
                {
                    query = query.Where(t => t.CreatedAt <= transactionCriteriaFilter.EndDate.Value);
                }
            }
            return query;
        }

        public async override Task<IEnumerable<TransactionResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Transaction> query, IFilterCriteria filter)
        {
            // Sắp xếp
            switch (filter.SortBy)
            {
                case SortType.Latest:
                    query = query.OrderByDescending(t => t.CreatedAt);
                    break;
                case SortType.Oldest:
                    query = query.OrderBy(t => t.CreatedAt);
                    break;
                case SortType.Ascending:
                    query = query.OrderBy(t => t.Amount);
                    break;
                case SortType.Descending:
                    query = query.OrderByDescending(t => t.Amount);
                    break;
                default:
                    query = query.OrderByDescending(t => t.CreatedAt);
                    break;
            }

            // Phân trang
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);

            var transactions = await query.ToListAsync();
            return transactions.Select(MapToDTO);
        }

        public override async Task<bool> UpdateAsync(TransactionRequestDTO dto)
        {
            try
            {
                // Lấy giao dịch theo mã giao dịch
                var query = await _transactionRepository.GetQueryableAsync();
                var entity = await query.FirstOrDefaultAsync(t => t.TransactionCode == dto.TransactionCode);

                if (entity == null)
                    return false;

                entity.UpdateFromRequestDto(dto);

                await _transactionRepository.UpdateAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật giao dịch mã: {dto.TransactionCode}");
                return false;
            }
        }

        protected override TransactionResponseDTO? MapToDTO(Transaction? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override Transaction? MapToEntity(TransactionRequestDTO dto)
        {
            if (dto == null) return null;
            return dto.ToEntity(null);
        }

        public override async Task<IEnumerable<TransactionResponseDTO?>> QueryAsync(string query)
        {
            var transactions = await _transactionRepository.FindAsync(t =>
                t.TransactionCode.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                t.Details.Contains(query, StringComparison.OrdinalIgnoreCase));
            return transactions.Select(MapToDTO);
        }

        public async Task<PaymentMethodResponseDTO?> AddPaymentMethodAsync(int userId, PaymentMethodRequestDTO paymentMethodRequestDTO)
        {
            var entity = paymentMethodRequestDTO.ToEntity();
            entity.UserId = userId;
            var paymentMethodEntity = await _paymentMethodRepository.AddAsync(entity);
            if (paymentMethodEntity == null)
                return null;
            return paymentMethodEntity.ToResponseDTO();
        }

        public async Task<IEnumerable<PaymentMethodResponseDTO?>> GetPaymentMethodsAsync()
        {
            var paymentMethods = await _paymentMethodRepository.GetAllAsync();
            return paymentMethods.Select(e => e.ToResponseDTO());
        }

        public async Task<PaymentMethodResponseDTO?> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
                return null;
            return paymentMethod.ToResponseDTO();
        }

        public async Task<PaymentMethodResponseDTO?> GetPaymentMethodByOrderIdAsync(int orderId)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(orderId);
            if (paymentMethod == null)
                return null;
            return paymentMethod.ToResponseDTO();
        }


    }
}