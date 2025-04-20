using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public class ProductRequestDTO : BaseRequestDTO
    {
        #region Thông tin cơ bản
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Mô tả sản phẩm là bắt buộc")]
        [MaxLength(500, ErrorMessage = "Mô tả sản phẩm không được vượt quá 500 ký tự")]
        public string Description { get; set; } = "";
        #endregion

        #region Thông tin giá & số lượng
        [Required(ErrorMessage = "Đơn giá sản phẩm là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá sản phẩm phải lớn hơn 0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng trong kho là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng trong kho không được âm")]
        public int StockQuantity { get; set; }
        #endregion

        #region Thông tin phân loại & trạng thái
        [Required(ErrorMessage = "Đơn vị tính là bắt buộc")]
        [EnumDataType(typeof(Unit))]
        public Unit Unit { get; set; } = Unit.Kg;
        [Required(ErrorMessage = "Cửa hàng là bắt buộc")]
        public int StoreId { get; set; }
        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Nguồn gốc sản phẩm là bắt buộc")]
        public string Origin { get; set; } = "";
        public bool? IsActive { get; set; }
        #endregion

        #region Hình ảnh
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        #endregion
    }
}