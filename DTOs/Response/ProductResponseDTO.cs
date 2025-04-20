using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Response
{
    public class ProductResponseDTO : BaseResponseDTO
    {
        #region Thông tin cơ bản
        public required string Name { get; set; }
        public required string Description { get; set; }
        #endregion
        
        #region Thông tin giá & số lượng
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required decimal SoldQuantity { get; set; }
        #endregion
        
        #region Thông tin phân loại & trạng thái
        public required Unit Unit { get; set; }
        public required int StoreId { get; set; }
        public int? CategoryId { get; set; }
        public required bool IsActive { get; set; }
        public required string Origin { get; set; }
        public required int TotalSoldQuantity { get; set; }
        #endregion
        
        #region Hình ảnh
        public required string ImageUrl { get; set; }
        #endregion

        public required decimal AverageRating { get; set; }
        
        #region Navigation Properties
        public StoreResponseDTO? Store { get; set; }
        public CategoryResponseDTO? Category { get; set; }
        public ICollection<ReviewResponseDTO>? Reviews { get; set; }
        #endregion
    }
}