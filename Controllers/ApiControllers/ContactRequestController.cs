using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Services;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ContactRequestController : ApiBaseController<ContactRequest, ContactRequestDTO, ContactRequestResponseDTO>
    {
        private readonly IContactRequestService _contactRequestService;

        public ContactRequestController(
            IContactRequestService contactRequestService,
            IJwtTokenService jwtTokenService,
            ILogger<ContactRequestController> logger) : base(contactRequestService, jwtTokenService, logger)
        {
            _contactRequestService = contactRequestService;
        }

        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search([FromBody] ContactRequestCriteriaFilter filter)
        {
            try
            {
                var query = await _contactRequestService.Query(filter);
                var total = await _contactRequestService.CountAsync(c => !c.IsDeleted);
                var items = await _contactRequestService.ApplyPagingAndSortingAsync(query, filter);

                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách yêu cầu liên hệ thành công",
                    totalCount = total,
                    page = filter.Page,
                    pageSize = filter.PageSize,
                    data = items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm kiếm yêu cầu liên hệ");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
    }
} 