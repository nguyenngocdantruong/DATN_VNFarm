using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Interfaces.Services;
using System.Collections.Generic;
using VNFarm_FinalFinal.Entities;
namespace VNFarm_FinalFinal.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApiBaseController<Category, CategoryRequestDTO, CategoryResponseDTO>
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : base(categoryService, logger)
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

