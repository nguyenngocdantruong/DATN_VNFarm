using Microsoft.AspNetCore.Mvc;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;

namespace VNFarm_FinalFinal.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessRegistrationController : ApiBaseController<BusinessRegistration, BusinessRegistrationRequestDTO, BusinessRegistrationResponseDTO>
    {
        private readonly IBusinessRegistrationService _businessRegistrationService;

        public BusinessRegistrationController(IBusinessRegistrationService businessRegistrationService, 
                                ILogger<BusinessRegistrationController> logger) : base(businessRegistrationService, logger)
        {
            _businessRegistrationService = businessRegistrationService;
        }

        /// <summary>
        /// Lấy đăng ký kinh doanh theo ID người dùng
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<BusinessRegistrationResponseDTO>> GetBusinessRegistrationByUserId(int userId)
        {
            var registration = await _businessRegistrationService.GetByUserIdAsync(userId);
            if (registration == null)
                return NotFound();

            return Ok(registration);
        }

        /// <summary>
        /// Lấy danh sách đăng ký kinh doanh theo bộ lọc
        /// </summary>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<BusinessRegistrationResponseDTO>>> GetBusinessRegistrationsByFilter([FromQuery] BusinessRegistrationCriteriaFilter filter)
        {
            var registrations = await _businessRegistrationService.Query(filter);
            var results = await _businessRegistrationService.ApplyPagingAndSortingAsync(registrations, filter);
            return Ok(results);
        }

        /// <summary>
        /// Lấy danh sách kết quả phê duyệt của đăng ký kinh doanh
        /// </summary>
        [HttpGet("{id}/approval-results")]
        public async Task<ActionResult<IEnumerable<RegistrationApprovalResultResponseDTO>>> GetRegistrationApprovalResults(int id)
        {
            var approvalResults = await _businessRegistrationService.GetRegistrationApprovalResultsAsync(id);
            return Ok(approvalResults);
        }

        /// <summary>
        /// Thêm kết quả phê duyệt cho đăng ký kinh doanh
        /// </summary>
        [HttpPost("{id}/approval-results")]
        public async Task<ActionResult<RegistrationApprovalResultResponseDTO>> AddRegistrationApprovalResult(int id, [FromBody] RegistrationApprovalResultRequestDTO approvalResultDTO, [FromQuery] int adminId)
        {
            if (id != approvalResultDTO.RegistrationId)
                return BadRequest("Business Registration ID không khớp");

            var result = await _businessRegistrationService.AddRegistrationApprovalResultAsync(approvalResultDTO, adminId);
            if (result == null)
                return BadRequest("Không thể thêm kết quả phê duyệt");

            return CreatedAtAction(nameof(GetRegistrationApprovalResults), new { id = id }, result);
        }

        /// <summary>
        /// Xác minh đăng ký kinh doanh
        /// </summary>
        [HttpPut("{id}/verify")]
        public async Task<ActionResult> VerifyRegistration(int id, [FromQuery] RegistrationStatus status, [FromQuery] string notes)
        {
            var success = await _businessRegistrationService.VerifyRegistrationAsync(id, status, notes);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}

