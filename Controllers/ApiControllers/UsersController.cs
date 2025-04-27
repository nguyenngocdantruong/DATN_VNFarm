using Microsoft.AspNetCore.Mvc;
using VNFarm.Helpers;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Services;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ApiBaseController<User, UserRequestDTO, UserResponseDTO>
    {
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;
        private readonly ITransactionService _transactionService;
        private readonly IOrderService _orderService;

        public UsersController(
            IUserService userService,
            IStoreService storeService,
            ITransactionService transactionService,
            IOrderService orderService,
            ILogger<UsersController> logger) : base(userService, logger)
        {
            _userService = userService;
            _storeService = storeService;
            _orderService = orderService;
            _transactionService = transactionService;
        }

        [HttpGet("by-email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest(new { success = false, message = "Email không được để trống." });

                var user = await _userService.GetByEmailAsync(email);
                if (user == null)
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng với email đã cung cấp." });

                return Ok(new { success = true, data = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin người dùng với email: {email}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpGet("check-email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest(new { success = false, message = "Email không được để trống." });

                var isUnique = await _userService.IsEmailUniqueAsync(email);
                return Ok(new { success = true, data = isUnique });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi kiểm tra tính duy nhất của email: {email}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers([FromQuery] UserCriteriaFilter filter)
        {
            try
            {
                var users = await _userService.Query(filter);
                var results = await _userService.ApplyPagingAndSortingAsync(users, filter);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm kiếm người dùng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái hoạt động của người dùng
        /// </summary>
        [HttpPut("{id}/active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetUserActive(int id, [FromQuery] bool isActive)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "ID không hợp lệ." });

                var success = await _userService.SetUserActiveAsync(id, isActive);
                if (!success)
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng." });

                return Ok(new { success = true, message = $"Đã cập nhật trạng thái hoạt động của người dùng thành {(isActive ? "kích hoạt" : "vô hiệu hóa")}." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật trạng thái hoạt động của người dùng với ID: {id}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] UserCriteriaFilter filter)
        {
            try
            {
                var users = await _userService.Query(filter);
                var totalCount = users.Count();
                var results = await _userService.ApplyPagingAndSortingAsync(users, filter);
                return Ok(new {
                    success = true,
                    data = results,
                    totalCount = totalCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm kiếm người dùng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
        protected override async Task<UserResponseDTO> IncludeNavigation(UserResponseDTO item)
        {
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(item.Id);
            item.Transactions = transactions;
            if(item.Role == Enums.UserRole.Seller){
                var store = await _storeService.GetStoreByUserIdAsync(item.Id);
                item.Store = store;
                if(item.Store != null){
                    var transactionsStore = await _transactionService.GetTransactionsByStoreIdAsync(item.Store.Id);
                    // Add store transactions to existing transactions instead of replacing them
                    if(item.Transactions == null) {
                        item.Transactions = transactionsStore;
                    } else {
                        item.Transactions = item.Transactions.Concat(transactionsStore).ToList();
                    }
                }
            }
            var paymentMethods = await _transactionService.GetPaymentMethodsByUserIdAsync(item.Id);
            item.PaymentMethods = paymentMethods;
            return await base.IncludeNavigation(item);
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetUserStats([FromQuery] int id){
            var user = await _userService.GetByIdAsync(id);
            if(user == null) return NotFound(new { success = false, data = "Không tìm thấy người dùng." });
            var transactionCount = await _transactionService.CountAsync(t => t.BuyerId == id);
            var orderCount = await _orderService.CountAsync(o => o.BuyerId == id);
            decimal totalRevenue = 0;
            try{
                var store = await _storeService.GetStoreByUserIdAsync(id);
                if(store != null){
                    orderCount += await _orderService.CountAsync(o => o.StoreId == store.Id);
                    totalRevenue = await _transactionService.CalculateTotalRevenueByStoreIdAsync(store.Id);
                }
            } catch (Exception ex){
                _logger.LogError(ex, $"Lỗi khi lấy thông tin cửa hàng của người dùng với ID: {id}");
            }
            return Ok(new {
                success = true,
                data = new {
                    userId = id,
                    transactionCount = transactionCount,
                    orderCount = orderCount,
                    totalRevenue = totalRevenue
                }
            });
        }
    }
} 