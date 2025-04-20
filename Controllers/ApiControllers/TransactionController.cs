using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Services;
using System.Collections.Generic;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ApiBaseController<Transaction, TransactionRequestDTO, TransactionResponseDTO>
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger) : base(transactionService, logger)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Lấy giao dịch theo người dùng
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetTransactionsByUser(int userId)
        {
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(userId);
            return Ok(transactions);
        }

        /// <summary>
        /// Lấy giao dịch theo cửa hàng
        /// </summary>
        [HttpGet("store/{storeId}")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetTransactionsByStore(int storeId)
        {
            var transactions = await _transactionService.GetTransactionsByStoreIdAsync(storeId);
            return Ok(transactions);
        }

        /// <summary>
        /// Lấy giao dịch theo đơn hàng
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<TransactionResponseDTO>> GetTransactionByOrder(int orderId)
        {
            var transaction = await _transactionService.GetTransactionByOrderIdAsync(orderId);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        /// <summary>
        /// Cập nhật trạng thái giao dịch
        /// </summary>
        [HttpPut("{id}/status/{status}")]
        public async Task<ActionResult> UpdateTransactionStatus(int id, TransactionStatus status)
        {
            await _transactionService.UpdateTransactionStatusAsync(id, status);
            return NoContent();
        }

        /// <summary>
        /// Lấy tổng doanh thu của cửa hàng trong khoảng thời gian
        /// </summary>
        [HttpGet("revenue/store/{storeId}")]
        public async Task<ActionResult<decimal>> GetTotalRevenue(int storeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var revenue = await _transactionService.GetTotalRevenueAsync(storeId, startDate, endDate);
            return Ok(new { Revenue = revenue });
        }

        /// <summary>
        /// Lấy giao dịch theo bộ lọc
        /// </summary>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetTransactionsByFilter([FromQuery] TransactionCriteriaFilter filter)
        {
            var transactions = await _transactionService.Query(filter);
            var results = await _transactionService.ApplyPagingAndSortingAsync(transactions, filter);
            return Ok(results);
        }

        /// <summary>
        /// Thêm phương thức thanh toán mới
        /// </summary>
        [HttpPost("payment-method/{userId}")]
        public async Task<ActionResult<PaymentMethodResponseDTO>> AddPaymentMethod(int userId, [FromBody] PaymentMethodRequestDTO paymentMethodRequestDTO)
        {
            var paymentMethod = await _transactionService.AddPaymentMethodAsync(userId, paymentMethodRequestDTO);
            if (paymentMethod == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetPaymentMethodById), new { id = paymentMethod.Id }, paymentMethod);
        }

        /// <summary>
        /// Lấy danh sách tất cả phương thức thanh toán
        /// </summary>
        [HttpGet("payment-method")]
        public async Task<ActionResult<IEnumerable<PaymentMethodResponseDTO>>> GetPaymentMethods()
        {
            var paymentMethods = await _transactionService.GetPaymentMethodsAsync();
            return Ok(paymentMethods);
        }

        /// <summary>
        /// Lấy phương thức thanh toán theo ID
        /// </summary>
        [HttpGet("payment-method/{id}")]
        public async Task<ActionResult<PaymentMethodResponseDTO>> GetPaymentMethodById(int id)
        {
            var paymentMethod = await _transactionService.GetPaymentMethodByIdAsync(id);
            if (paymentMethod == null)
                return NotFound();

            return Ok(paymentMethod);
        }

        /// <summary>
        /// Lấy phương thức thanh toán theo đơn hàng
        /// </summary>
        [HttpGet("payment-method/order/{orderId}")]
        public async Task<ActionResult<PaymentMethodResponseDTO>> GetPaymentMethodByOrder(int orderId)
        {
            var paymentMethod = await _transactionService.GetPaymentMethodByOrderIdAsync(orderId);
            if (paymentMethod == null)
                return NotFound();

            return Ok(paymentMethod);
        }
    }
}
