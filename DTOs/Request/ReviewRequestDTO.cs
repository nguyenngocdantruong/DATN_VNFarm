using System;
using System.ComponentModel.DataAnnotations;
namespace VNFarm.DTOs.Request
{
    public class ReviewRequestDTO: BaseRequestDTO
    {
        [Required(ErrorMessage = "Mã người dùng là bắt buộc")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Điểm đánh giá là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5 sao")]
        public int Rating { get; set; }
        public string? Content { get; set; } = string.Empty;
        public string? ShopResponse { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderId { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
    }
}