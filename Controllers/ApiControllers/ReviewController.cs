using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Interfaces.Services;

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
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByOrderId(int orderId)
        {
            var reviews = await _reviewService.GetReviewByOrderIdAsync(orderId);
            return Ok(reviews);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }
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
    }
}
