using VNFarm.Enums;

namespace VNFarm.DTOs.Response
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
        public required int ReviewStar1Count { get; set; }
        public required int ReviewStar2Count { get; set; }
        public required int ReviewStar3Count { get; set; }
        public required int ReviewStar4Count { get; set; }
        public required int ReviewStar5Count { get; set; }
        
        #region Navigation Properties
        public StoreResponseDTO? Store { get; set; }
        public CategoryResponseDTO? Category { get; set; }
        public IEnumerable<ReviewResponseDTO?>? Reviews { get; set; }
        #endregion
        public override string ToString()
        {
            return $"Product ID: {Id}\n" +
                   $"Name: {Name}\n" +
                   $"Description: {Description}\n" +
                   $"Price: {Price}\n" +
                   $"Stock Quantity: {StockQuantity}\n" +
                   $"Sold Quantity: {SoldQuantity}\n" +
                   $"Unit: {Unit}\n" +
                   $"Store ID: {StoreId}\n" +
                   $"Category ID: {CategoryId}\n" +
                   $"Is Active: {IsActive}\n" +
                   $"Origin: {Origin}\n" +
                   $"Total Sold Quantity: {TotalSoldQuantity}\n" +
                   $"Image URL: {ImageUrl}\n" +
                   $"Average Rating: {AverageRating}\n" +
                   $"Store: {(Store != null ? "Set" : "Null")}\n" +
                   $"Category: {(Category != null ? "Set" : "Null")}\n" +
                   $"Reviews: {(Reviews != null && Reviews.Any() ? $"{Reviews.Count()} reviews" : "No reviews")}";
        }
    }
}