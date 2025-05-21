using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using VNFarm.Enums;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.DTOs.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VNFarm.Services.Interfaces;
namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ApiBaseController<Notification, NotificationRequestDTO, NotificationResponseDTO>
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, IJwtTokenService jwtTokenService, ILogger<NotificationController> logger) : base(notificationService, jwtTokenService, logger)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Lấy thông báo theo người dùng
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDTO>>> GetNotificationsByUser(int userId)
        {
            var currentUserId = GetCurrentUserId();
            if(currentUserId == null || currentUserId != userId){
                return Unauthorized(new {
                    totalCount = 0,
                    data = new List<NotificationResponseDTO>(),
                    success = false,
                    message = "Bạn không có quyền truy cập vào thông báo của người dùng này"
                });
            }
            var notifications = await _notificationService.GetByUserIdAsync(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// Gửi thông báo đến người dùng
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("send/user/{userId}")]
        public async Task<ActionResult> SendToUser(int userId, [FromBody] NotificationRequestDTO request)
        {
            var success = await _notificationService.SendToUserAsync(userId, request.Content, request.Type);
            if (!success)
                return BadRequest("Không thể gửi thông báo");

            return Ok();
        }

        /// <summary>
        /// Gửi thông báo đến tất cả người dùng
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("send/all")]
        public async Task<ActionResult> SendToAllUsers([FromBody] NotificationRequestDTO request)
        {
            var success = await _notificationService.SendToAllUsersAsync(request.Content, request.Type);
            if (!success)
                return BadRequest("Không thể gửi thông báo");

            return Ok();
        }

        /// <summary>
        /// Xóa tất cả thông báo của người dùng
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("user/{userId}")]
        public async Task<ActionResult> DeleteAllForUser(int userId)
        {
            var success = await _notificationService.DeleteAllForUserAsync(userId);
            if (!success)
                return BadRequest("Không thể xóa thông báo");

            return NoContent();
        }
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDTO>>> Filter([FromBody] NotificationCriteriaFilter filter)
        {
            if(filter.UserId.HasValue){
                var currentUserId = GetCurrentUserId();
                if((currentUserId == null || currentUserId != filter.UserId) && !IsCurrentUserAdmin){
                    return Unauthorized(new {
                        totalCount = 0,
                        data = new List<NotificationResponseDTO>(),
                        success = false,
                        message = "Bạn không có quyền truy cập vào thông báo của người dùng này"
                    });
                }
            }
            var notifications = await _notificationService.Query(filter);
            var totalCount = await notifications.CountAsync();
            var result = await _notificationService.ApplyPagingAndSortingAsync(notifications, filter);
            return Ok(new {
                totalCount = totalCount,
                data = result,
                success = true,
                message = "Lấy thông báo thành công"
            });
        }
    }
}
