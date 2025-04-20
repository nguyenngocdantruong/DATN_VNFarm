using Microsoft.AspNetCore.Mvc;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Helpers;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.DTOs.Filters;

namespace VNFarm_FinalFinal.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ApiBaseController<User, UserRequestDTO, UserResponseDTO>
    {
        private readonly IUserService _userService;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger) : base(userService, logger)
        {
            _userService = userService;
        }

        [HttpGet("by-email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest(new { success = false, message = "Email không được để trống." });

                var user = await _userService.GetByEmailAsync(email);
                if (user == null)
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng với email đã cung cấp." });

                return Ok(new { success = true, data = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin người dùng với email: {email}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpGet("check-email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest(new { success = false, message = "Email không được để trống." });

                var isUnique = await _userService.IsEmailUniqueAsync(email);
                return Ok(new { success = true, data = isUnique });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi kiểm tra tính duy nhất của email: {email}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers([FromQuery] UserCriteriaFilter filter)
        {
            try
            {
                var users = await _userService.Query(filter);
                var results = await _userService.ApplyPagingAndSortingAsync(users, filter);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm kiếm người dùng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái hoạt động của người dùng
        /// </summary>
        [HttpPut("{id}/active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetUserActive(int id, [FromQuery] bool isActive)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "ID không hợp lệ." });

                var success = await _userService.SetUserActiveAsync(id, isActive);
                if (!success)
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng." });

                return Ok(new { success = true, message = $"Đã cập nhật trạng thái hoạt động của người dùng thành {(isActive ? "kích hoạt" : "vô hiệu hóa")}." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật trạng thái hoạt động của người dùng với ID: {id}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
    }
} 