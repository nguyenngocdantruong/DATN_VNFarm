using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using VNFarm.Enums;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Services;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ApiBaseController<Notification, NotificationRequestDTO, NotificationResponseDTO>
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger) : base(notificationService, logger)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Lấy thông báo theo người dùng
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDTO>>> GetNotificationsByUser(int userId)
        {
            var notifications = await _notificationService.GetByUserIdAsync(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// Gửi thông báo đến người dùng
        /// </summary>
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
        [HttpDelete("user/{userId}")]
        public async Task<ActionResult> DeleteAllForUser(int userId)
        {
            var success = await _notificationService.DeleteAllForUserAsync(userId);
            if (!success)
                return BadRequest("Không thể xóa thông báo");

            return NoContent();
        }
    }
}
