using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Services;

namespace VNFarm_FinalFinal.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ApiBaseController<Product, ProductRequestDTO, ProductResponseDTO>
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService, ILogger<ProductController> logger) : base(productService, logger)
        {
            _productService = productService;
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
            _logger.LogInformation("Filter: {Filter}", filter);
            var query = await _productService.Query(filter);
            query = query.Include(m => m.Category);
            var totalCount = query.Count();
            var products = await _productService.ApplyPagingAndSortingAsync(query, filter);
            _logger.LogInformation("Products: {Products}", products);
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
            var reviews = await _productService.GetReviewsAsync(productId);
            return Ok(reviews);
        }
    }
}

