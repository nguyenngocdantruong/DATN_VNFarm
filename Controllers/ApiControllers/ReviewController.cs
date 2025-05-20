using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Interfaces.Services;
using VNFarm.DTOs.Filters;
using Microsoft.EntityFrameworkCore;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ApiBaseController<Review, ReviewRequestDTO, ReviewResponseDTO>
    {
        private readonly IReviewService _reviewService;
        private readonly IOrderService _orderService;   
        public ReviewController(IReviewService reviewService, IOrderService orderService, IJwtTokenService jwtTokenService, ILogger<ApiBaseController<Review, ReviewRequestDTO, ReviewResponseDTO>> logger) : base(reviewService, jwtTokenService, logger)
        {
            _reviewService = reviewService;
            _orderService = orderService;
        }
        
        /// <summary>
        /// Thêm đánh giá sản phẩm
        /// </summary>
        public override async Task<IActionResult> AddAsync([FromForm] ReviewRequestDTO dto)
        {
            var userId = GetCurrentUserId();
            if(userId == null)
            {
                return BadRequest(new { success = false, message = "Bạn cần đăng nhập để đánh giá sản phẩm" });
            }
            dto.UserId = userId.Value;
            var order = await _orderService.GetByIdAsync(dto.OrderId);
            if(order == null)
            {
                return BadRequest(new { success = false, message = "Đơn hàng không tồn tại" });
            }
            if(order.Status != OrderStatus.Completed)
            {
                return BadRequest(new { success = false, message = "Đơn hàng chưa hoàn thành" });
            }
            if(order.OrderItems.Any(x => x.ProductId != dto.ProductId))
            {
                return BadRequest(new { success = false, message = "Sản phẩm không thuộc đơn hàng" });
            }
            var reviews = await _reviewService.GetReviewByOrderIdAsync(dto.OrderId);
            var anyReview = reviews.Any(x => x != null && x.ProductId == dto.ProductId && x.UserId == userId.Value);
            if(anyReview)
            {
                return BadRequest(new { success = false, message = "Bạn đã đánh giá sản phẩm này rồi" });
            }
            // Tới đây là hợp lệ
            try
            {
                var uploadedDto = await UploadFile(dto);
                if (uploadedDto == null)
                    return BadRequest(new { success = false, message = "Không thể tải lên tệp." });
                var result = await _reviewService.AddAsync(uploadedDto);
                if (result == null)
                    return BadRequest(new { success = false, message = "Không thể thêm mới dữ liệu." });
                var averageRating = await _reviewService.CalculateAverageRatingAsync(dto.ProductId, dto.Rating);
                _logger.LogInformation("Đánh giá trung bình mới của sản phẩm: {averageRating}", averageRating);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, new { success = true, data = result, averageRating = averageRating });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm mới dữ liệu");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
        
        /// <summary>
        /// Lấy đánh giá theo đơn hàng
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByOrderId(int orderId)
        {
            var reviews = await _reviewService.GetReviewByOrderIdAsync(orderId);
            return Ok(new { success = true, data = reviews });
        }

        /// <summary>
        /// Lấy đánh giá theo sản phẩm
        /// </summary>
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            return Ok(new { success = true, data = reviews });
        }
        
        /// <summary>
        /// Tải file lên
        /// </summary>
        protected override async Task<ReviewRequestDTO> UploadFile(ReviewRequestDTO req)
        {
            if (req.ImageFile == null || req.ImageFile.Length == 0){
                _logger.LogWarning("ReviewController: No file uploaded.");
                return req;
            }
            if (req.ImageFile.Length > 1048576 * 10){
                _logger.LogWarning("ReviewController: File size exceeds 10MB limit.");
                return req;
            }
            var url = await FileUpload.UploadFile(req.ImageFile, FileUpload.ReviewFolder);
            req.ImageUrl = url;
            return req;
        }
        
        #region OrderItem Reviews
        
        /// <summary>
        /// Kiểm tra xem người dùng có thể đánh giá sản phẩm trong đơn hàng hay không
        /// </summary>
        [HttpGet("check/{orderId}/{productId}")]
        public async Task<IActionResult> CheckCanReview(int orderId, int productId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này" });
            }
            
            var canReview = await _reviewService.CanReviewOrderItemAsync(orderId, productId, userId.Value);
            return Ok(new { success = true, canReview = canReview });
        }
        
        /// <summary>
        /// Thêm đánh giá cho sản phẩm trong đơn hàng
        /// </summary>
        [HttpPost("order-item")]
        public async Task<IActionResult> AddReviewForOrderItem([FromForm] ReviewRequestDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để đánh giá sản phẩm" });
            }
            
            dto.UserId = userId.Value;
            
            try
            {
                // Tải lên hình ảnh nếu có
                var uploadedDto = await UploadFile(dto);
                if (uploadedDto == null)
                {
                    return BadRequest(new { success = false, message = "Không thể tải lên tệp." });
                }
                
                // Thêm đánh giá
                var result = await _reviewService.AddReviewForOrderItemAsync(uploadedDto);
                if (result == null)
                {
                    return BadRequest(new { success = false, message = "Không thể thêm đánh giá. Vui lòng kiểm tra lại thông tin." });
                }
                
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm đánh giá cho sản phẩm trong đơn hàng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
        
        /// <summary>
        /// Lấy danh sách các sản phẩm trong đơn hàng mà người dùng có thể đánh giá
        /// </summary>
        [HttpGet("reviewable/{orderId}")]
        public async Task<IActionResult> GetReviewableOrderItems(int orderId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này" });
            }
            
            var items = await _reviewService.GetReviewableOrderItemsAsync(orderId, userId.Value);
            return Ok(new { success = true, data = items });
        }
        
        /// <summary>
        /// Lấy danh sách đánh giá của người dùng hiện tại
        /// </summary>
        [HttpGet("my-reviews")]
        public async Task<IActionResult> GetMyReviews()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này" });
            }
            
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId.Value);
            return Ok(new { success = true, data = reviews });
        }
        
        #endregion

        /// <summary>
        /// Lọc đánh giá theo nhiều tiêu chí
        /// </summary>
        [HttpPost("filter")]
        public async Task<IActionResult> FilterReviews([FromBody] ReviewFilterCriteria filter)
        {
            try
            {
                var query = await _reviewService.Query(filter);
                var count = await query.CountAsync();
                var results = await _reviewService.ApplyPagingAndSortingAsync(query, filter);
                
                return Ok(new { 
                    success = true, 
                    data = results,
                    totalCount = count,
                    page = filter.Page,
                    pageSize = filter.PageSize,
                    totalPages = (int)Math.Ceiling((double)count / filter.PageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lọc đánh giá");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Lấy danh sách đánh giá của một người dùng cụ thể
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsByUserId(int userId, [FromQuery] ReviewFilterCriteria filter = null)
        {
            try
            {
                if (filter == null)
                {
                    filter = new ReviewFilterCriteria();
                }
                
                filter.UserId = userId;
                
                var query = await _reviewService.Query(filter);
                var count = await query.CountAsync();
                var results = await _reviewService.ApplyPagingAndSortingAsync(query, filter);
                
                return Ok(new { 
                    success = true, 
                    data = results,
                    totalCount = count,
                    page = filter.Page,
                    pageSize = filter.PageSize,
                    totalPages = (int)Math.Ceiling((double)count / filter.PageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy đánh giá của người dùng #{UserId}", userId);
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
    }
}
