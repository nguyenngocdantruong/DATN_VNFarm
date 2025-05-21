using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Helpers;
using VNFarm.Services.Interfaces;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ApiBaseController<Product, ProductRequestDTO, ProductResponseDTO>
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService,ICategoryService categoryService, IStoreService storeService, IUserService userService, IJwtTokenService jwtTokenService, ILogger<ProductController> logger) : base(productService, jwtTokenService, logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _storeService = storeService;
            _userService = userService;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm theo danh mục (id)
        /// </summary>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetByCategory(int categoryId)
        {
            var products = await _productService.GetByCategoryAsync(categoryId);
            return Ok(products);
        }

        /// <summary>
        /// Lấy danh sách sản phẩm theo cửa hàng (id)
        /// </summary>
        [HttpGet("store/{storeId}")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetByStore(int storeId)
        {
            var products = await _productService.GetByStoreAsync(storeId);
            return Ok(products);
        }

        /// <summary>
        /// Lấy danh sách sản phẩm bán chạy nhất
        /// </summary>
        [HttpGet("top-selling")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetTopSellingProducts([FromQuery] int page = 1, [FromQuery] int count = 10)
        {
            var products = await _productService.GetTopSellingProductsAsync(page, count);
            return Ok(products);
        }

        /// <summary>
        /// Cập nhật số lượng tồn kho của sản phẩm
        /// </summary>
        [HttpPut("{productId}/stock")]
        public async Task<IActionResult> UpdateStock(int productId, [FromBody] int quantity)
        {
            await _productService.UpdateStockAsync(productId, quantity);
            return NoContent();
        }

        /// <summary>
        /// Lấy danh sách sản phẩm theo bộ lọc
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts([FromBody] ProductCriteriaFilter filter)
        {
            var query = await _productService.Query(filter);
            query = query.Include(m => m.Category);
            var totalCount = query.Count();
            var products = await _productService.ApplyPagingAndSortingAsync(query, filter);
            return Ok(new {
                success = true,
                data = products,
                totalCount = totalCount
            });
        }

        /// <summary>
        /// Lấy danh sách đánh giá của sản phẩm
        /// </summary>
        [HttpGet("{productId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviews(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm." });
            var reviews = await _productService.GetReviewsAsync(productId);
            // Include user
            foreach (var review in reviews)
            {
                if(review != null)
                {
                    var user = await _userService.GetByIdAsync(review.UserId);
                    if (user != null)
                    {
                        review.User = user;
                    }
                }
            }
            return Ok(new { success = true, data = new {
                product = product,
                reviews = reviews,
            } });
        }

        protected override async Task<ProductResponseDTO> IncludeNavigation(ProductResponseDTO item)
        {
            // _logger.LogWarning("IncludeNavigation: {Item}", item.ToString());
            item.Reviews = (await _productService.GetReviewsAsync(item.Id)).Take(1).ToList();
            if(item.CategoryId != null)
                item.Category = await _categoryService.GetByIdAsync(item.CategoryId.Value);
            return await base.IncludeNavigation(item);
        }

        protected override async Task<ProductRequestDTO> UploadFile(ProductRequestDTO req)
        {
            if (req.ImageFile == null || req.ImageFile.Length == 0){
                _logger.LogWarning("ProductController: No file uploaded.");
                return req;
            }
            if (req.ImageFile.Length > 1048576 * 10){
                _logger.LogWarning("ProductController: File size exceeds 10MB limit.");
                return req;
            }
            var url = await FileUpload.UploadFile(req.ImageFile, FileUpload.ProductFolder);
            req.ImageUrl = url;
            return req;
        }
    }
}

