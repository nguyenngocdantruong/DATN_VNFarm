using Microsoft.EntityFrameworkCore;
using VNFarm.DTOs.Filters;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Interfaces.Repositories;
using VNFarm.Interfaces.Services;
using VNFarm.Helpers;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Mappers;

namespace VNFarm.Services
{
    public class ProductService : BaseService<Product, ProductRequestDTO, ProductResponseDTO>, IProductService
    {
        #region Fields & Constructor
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IRepository<Product> repository,
            IProductRepository productRepository,
            IRepository<Review> reviewRepository,
            ILogger<ProductService> logger) : base(repository)
        {
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _logger = logger;
        }
        #endregion

        #region Base Service Implementation
        protected override ProductResponseDTO? MapToDTO(Product? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override Product? MapToEntity(ProductRequestDTO dto)
        {
            return dto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(ProductRequestDTO dto)
        {
            var entity = await _productRepository.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            if (dto.ImageFile != null)
            {
                var fileUrl = await FileUpload.UploadFile(dto.ImageFile, FileUpload.ProductFolder);
                dto.ImageUrl = fileUrl;
            }

            entity.UpdateFromRequestDto(dto);
            return await _productRepository.UpdateAsync(entity);
        }

        public override async Task<IEnumerable<ProductResponseDTO?>> QueryAsync(string query)
        {
            var products = await _productRepository.FindAsync(
                p => p.Name.Contains(query) ||
                     p.Description.Contains(query) ||
                     p.Origin.Contains(query)
            );
            return products.Select(MapToDTO);
        }
        #endregion

        #region Product Query Methods
        public override async Task<IQueryable<Product>> Query(IFilterCriteria filter)
        {
            var query = await _productRepository.GetQueryableAsync();
            if (filter is ProductCriteriaFilter productCriteriaFilter)
            {
                // Apply search filter
                if (!string.IsNullOrEmpty(productCriteriaFilter.SearchTerm))
                {
                    query = query.Where(p =>
                        p.Name.Contains(productCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Description.Contains(productCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase)
                    );
                }

                //Apply storeid filter
                if (productCriteriaFilter.StoreId.HasValue)
                {
                    query = query.Where(p => p.StoreId == productCriteriaFilter.StoreId.Value);
                }

                // Apply category filter
                if (productCriteriaFilter.CategoryId != -999)
                {
                    query = query.Where(p => p.CategoryId == productCriteriaFilter.CategoryId);
                }

                // Apply price filters
                if (productCriteriaFilter.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= productCriteriaFilter.MinPrice.Value);
                }
                if (productCriteriaFilter.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= productCriteriaFilter.MaxPrice.Value);
                }

                // Apply status filters
                if (productCriteriaFilter.IsActive.HasValue)
                {
                    query = query.Where(p => p.IsActive == productCriteriaFilter.IsActive.Value);
                }
                if (productCriteriaFilter.IsInStock.HasValue)
                {
                    if (productCriteriaFilter.IsInStock.Value)
                    {
                        query = query.Where(p => p.StockQuantity > 0);
                    }
                    else
                    {
                        query = query.Where(p => p.StockQuantity <= 0);
                    }
                }

                // Apply unit filter
                if (productCriteriaFilter.Unit != Unit.All)
                {
                    query = query.Where(p => p.Unit == productCriteriaFilter.Unit);
                }

                // Apply origin filter
                if (!string.IsNullOrEmpty(productCriteriaFilter.Origin))
                {
                    query = query.Where(p =>
                        p.Origin.Contains(productCriteriaFilter.Origin, StringComparison.OrdinalIgnoreCase)
                    );
                }
                return query;
            }
            else
            {
                throw new ArgumentException("Filter truyền vào không phải là ProductCriteriaFilter");
            }
        }
        public override async Task<IEnumerable<ProductResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Product> query, IFilterCriteria filter)
        {
            if (filter is ProductCriteriaFilter productCriteriaFilter)
            {
                // Apply sorting
                query = productCriteriaFilter.SortBy switch
                {
                    SortType.Ascending => query.OrderBy(p => p.Name),
                    SortType.Descending => query.OrderByDescending(p => p.Name),
                    SortType.Latest => query.OrderByDescending(p => p.CreatedAt),
                    SortType.Oldest => query.OrderBy(p => p.CreatedAt),
                    SortType.AscendingPrice => query.OrderBy(p => p.Price),
                    SortType.DescendingPrice => query.OrderByDescending(p => p.Price),
                    _ => query
                };
            }
            else
            {
                throw new ArgumentException("Filter truyền vào không phải là ProductCriteriaFilter");
            }
            // Apply paging
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(productCriteriaFilter.PageSize);
            return (await query.ToListAsync()).Select(MapToDTO);
        }

        public async Task<IEnumerable<ProductResponseDTO?>> GetByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _productRepository.FindAsync(p => p.CategoryId == categoryId);
                return products.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy sản phẩm theo danh mục: {categoryId}");
                return Enumerable.Empty<ProductResponseDTO>();
            }
        }

        public async Task<IEnumerable<ProductResponseDTO?>> GetByStoreAsync(int storeId)
        {
            try
            {
                var products = await _productRepository.FindAsync(p => p.StoreId == storeId);
                return products.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy sản phẩm theo cửa hàng: {storeId}");
                return Enumerable.Empty<ProductResponseDTO>();
            }
        }

        public async Task<IEnumerable<ProductResponseDTO?>> GetTopSellingProductsAsync(int page, int count)
        {
            var topProducts = await _productRepository.GetTopSellingProductsAsync(page, count);
            return topProducts.Select(MapToDTO);
        }

        public async Task<IEnumerable<ProductResponseDTO?>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _productRepository.FindAsync(p => p.CategoryId == categoryId);
                return products.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy sản phẩm theo danh mục: {categoryId}");
                return Enumerable.Empty<ProductResponseDTO>();
            }
        }

        public async Task<IEnumerable<ProductResponseDTO?>> GetProductsByStoreAsync(int storeId)
        {
            try
            {
                var products = await _productRepository.FindAsync(p => p.StoreId == storeId);
                return products.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy sản phẩm theo cửa hàng: {storeId}");
                return Enumerable.Empty<ProductResponseDTO>();
            }
        }
        #endregion

        #region Product Management
        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                await _productRepository.UpdateStockAsync(productId, quantity);
            }
        }
        #endregion

        #region Review Management
        public async Task<IEnumerable<ReviewResponseDTO?>> GetReviewsAsync(int productId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return Enumerable.Empty<ReviewResponseDTO>();

                var reviews = await _reviewRepository.FindAsync(r => r.ProductId == product.Id);
                return reviews.Select(r => r.ToResponseDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy đánh giá cho sản phẩm: {productId}");
                return Enumerable.Empty<ReviewResponseDTO>();
            }
        }
        #endregion
    }
}