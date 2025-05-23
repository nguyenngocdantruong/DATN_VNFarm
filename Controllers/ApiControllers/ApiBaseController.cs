using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNFarm.Helpers;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Services.Interfaces;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class ApiBaseController<TEntity, TReq, TRes>(IService<TEntity, TReq, TRes> service, IJwtTokenService jwtTokenService, ILogger<ApiBaseController<TEntity, TReq, TRes>> logger) : ControllerBase where TEntity : BaseEntity where TReq : BaseRequestDTO where TRes : BaseResponseDTO
    {
        protected readonly IService<TEntity, TReq, TRes> _service = service;
        protected readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        protected readonly ILogger<ApiBaseController<TEntity, TReq, TRes>> _logger = logger;

        [HttpPost]
        public virtual async Task<IActionResult> AddAsync([FromForm] TReq dto)
        {
            try
            {
                var uploadedDto = await UploadFile(dto);
                if (uploadedDto == null)
                    return BadRequest(new { success = false, message = "Không thể tải lên tệp." });
                var result = await _service.AddAsync(uploadedDto);
                if (result == null)
                    return BadRequest(new { success = false, message = "Không thể thêm mới dữ liệu." });
                return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm mới dữ liệu");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Hàm thực hiện tải lên file và cập nhật thông tin về file url trong DTO.
        /// </summary>
        /// <param name="req">Request DTO gửi từ client có thể kèm theo file</param>
        /// <returns>Kết quả sau khi đã upload file và update info</returns>
        protected virtual async Task<TReq> UploadFile(TReq req)
        {
            return await Task.Run(() => req);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAsync()
        {
            try
            {
                var count = await _service.CountAsync();
                return Ok(new { success = true, data = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đếm số lượng dữ liệu");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "ID không hợp lệ." });

                var exists = await _service.ExistsAsync(id);
                if (!exists)
                    return NotFound(new { success = false, message = "Không tìm thấy dữ liệu." });

                var result = await _service.DeleteAsync(id);
                if (!result)
                    return BadRequest(new { success = false, message = "Không thể xóa dữ liệu." });

                return Ok(new { success = true, message = "Đã xóa dữ liệu thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa dữ liệu với ID: {id}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpGet("exists/{id}")]
        public async Task<IActionResult> ExistsAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "ID không hợp lệ." });

                var exists = await _service.ExistsAsync(id);
                return Ok(new { success = true, data = exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi kiểm tra dữ liệu với ID: {id}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(new { success = true, data = items });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả dữ liệu");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "ID không hợp lệ." });

                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound(new { success = false, message = "Không tìm thấy dữ liệu." });
                item = await IncludeNavigation(item);
                return Ok(new { success = true, data = item });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy dữ liệu với ID: {id}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        protected virtual async Task<TRes> IncludeNavigation(TRes item)
        {
            return await Task.Run(() => item);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(int id, [FromForm] TReq dto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "ID không hợp lệ." });

                if (dto.Id != id)
                    return BadRequest(new { success = false, message = "ID không khớp với dữ liệu cần cập nhật." });

                var exists = await _service.ExistsAsync(id);
                if (!exists)
                    return NotFound(new { success = false, message = "Không tìm thấy dữ liệu." });
                var uploadedDto = await UploadFile(dto);
                if (uploadedDto == null)
                    return BadRequest(new { success = false, message = "Không thể tải lên tệp." });
                var result = await _service.UpdateAsync(uploadedDto);
                if (!result)
                    return BadRequest(new { success = false, message = "Không thể cập nhật dữ liệu." });

                var updatedItem = await _service.GetByIdAsync(id);
                return Ok(new { success = true, message = "Cập nhật dữ liệu thành công.", data = updatedItem });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật dữ liệu với ID: {id}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
        [HttpGet("query")]
        public async Task<IActionResult> QueryAsync(string query)
        {
            try
            {
                var items = await _service.QueryAsync(query);
                return Ok(new { success = true, data = items });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tìm kiếm dữ liệu với query: {query}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        protected int? GetCurrentUserId()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return null;

            var token = authHeader.Substring("Bearer ".Length).Trim();
            return _jwtTokenService.GetUserIdFromToken(token);
        }

        private string? GetCurrentUserRole()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return null;
            var token = authHeader.Substring("Bearer ".Length).Trim();
            return _jwtTokenService.GetRoleFromToken(token);
        }
        protected bool IsCurrentUserAdmin => GetCurrentUserRole() == "Admin";
        protected bool IsCurrentUserCustomer => GetCurrentUserRole() == "Buyer" || GetCurrentUserRole() == "User";
        protected bool IsCurrentUserSeller => GetCurrentUserRole() == "Seller";
    }
}
