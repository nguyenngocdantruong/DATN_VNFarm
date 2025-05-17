using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Services;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApiBaseController<Category, CategoryRequestDTO, CategoryResponseDTO>
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, IJwtTokenService jwtTokenService, ILogger<CategoryController> logger) : base(categoryService, jwtTokenService, logger)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Lấy danh sách danh mục theo bộ lọc
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetCategoriesByFilter([FromBody] CategoryCriteriaFilter filter)
        {
            var categories = await _categoryService.Query(filter);
            var results = await _categoryService.ApplyPagingAndSortingAsync(categories, filter);
            return Ok(results);
        }
    }
}

