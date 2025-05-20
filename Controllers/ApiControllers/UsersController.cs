using Microsoft.AspNetCore.Mvc;
using VNFarm.Helpers;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Services;
using VNFarm.Enums;

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
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IProductService _productService;

        public UsersController(
            IUserService userService,
            IStoreService storeService,
            ITransactionService transactionService,
            IOrderService orderService,
            IJwtTokenService jwtTokenService,
            IProductService productService,
            ILogger<UsersController> logger) : base(userService, jwtTokenService, logger)
        {
            _userService = userService;
            _storeService = storeService;
            _orderService = orderService;
            _transactionService = transactionService;
            _jwtTokenService = jwtTokenService;
            _productService = productService;
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
            var orders = await _orderService.FindAsync(o => o.BuyerId == id);
            return Ok(new {
                success = true,
                data = new {
                    userId = id,
                    orderCount = orders.Count(),
                    totalRevenue = orders.Sum(o => o.TotalAmount)
                }
            });
        }
        [HttpPost("user-info")]
        public async Task<IActionResult> GetUserInfo([FromBody] string token, [FromQuery] int userId){
            var user = await _userService.GetByIdAsync(userId);
            if(user == null) return NotFound(new { success = false, message = "Không tìm thấy người dùng." });
            var isValid = _jwtTokenService.GetUserIdFromToken(token) == userId;
            if(!isValid) return Unauthorized(new { success = false, message = "Token không hợp lệ." });
            return Ok(new {
                success = true,
                message = "Lấy thông tin người dùng thành công.",
                data = user
            });
        }
        protected override async Task<UserRequestDTO> UploadFile(UserRequestDTO req)
        {
            if (req.ImageFile == null || req.ImageFile.Length == 0){
                _logger.LogWarning("UsersController: No file uploaded.");
                return req;
            }
            if (req.ImageFile.Length > 1048576 * 10){
                _logger.LogWarning("UsersController: File size exceeds 10MB limit.");
                return req;
            }
            var url = await FileUpload.UploadFile(req.ImageFile, FileUpload.UserFolder);
            req.ImageUrl = url;
            return req;
        }
        public override async Task<IActionResult> UpdateAsync(int id, [FromForm] UserRequestDTO dto)
        {
            if(!string.IsNullOrEmpty(dto.PasswordNew)){
                dto.PasswordNew = AuthUtils.GenerateMd5Hash(dto.PasswordNew);
                _logger.LogInformation($"UsersController: Đang cập nhật mật khẩu mới cho người dùng {id}: {dto.PasswordNew}");
            }
            return await base.UpdateAsync(id, dto);
        }
        [HttpGet("{userId}/get-statistics-user")]
        public async Task<IActionResult> GetStatisticsUser(int userId)
        {
            // Kiểm tra xem có phải là người dùng hiện tại không
            var currentUserId = GetCurrentUserId();
            if(currentUserId == null){
                return Unauthorized(new {
                    success = false,
                    message = "Bạn cần đăng nhập để xem thông tin người dùng"
                });
            }
            var user = await _userService.GetByIdAsync(userId);
            if(user == null || 
                (userId != currentUserId && !IsCurrentUserAdmin)){
                return Unauthorized(new {
                    success = false,
                    message = "Bạn không có quyền truy cập vào thông tin người dùng này"
                });
            }

            var totalOrders = await _orderService.CountAsync(m => m.IsDeleted == false && m.BuyerId == userId);
            var totalRenenvue = (await _orderService.FindAsync(m => m.IsDeleted == false && m.BuyerId == userId)).Sum(m => m != null ? m.TotalAmount : 0);
            var listRenenvueInYear = new List<decimal>();
            var currentYear = DateTime.Now.Year;
            
            for(int month = 1; month <= 12; month++) {
                var startDate = new DateTime(currentYear, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                
                var monthlyRevenue = (await _orderService.FindAsync(o => 
                    o.IsDeleted == false && 
                    o.BuyerId == userId &&
                    o.CreatedAt >= startDate && 
                    o.CreatedAt <= endDate &&
                    o.PaymentStatus == PaymentStatus.Paid
                )).Sum(o => o != null ? o.TotalAmount : 0);
                
                listRenenvueInYear.Add(monthlyRevenue);
            }
            return Ok(new 
            {
                success = true,
                data = new
                {
                    totalOrders,
                    totalRenenvue,
                    listRenenvueInYear
                },
                userId = userId
            });
        }
    }
} 