using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using PusherServer;
using Microsoft.EntityFrameworkCore;
using VNFarm.Mappers;
using VNFarm.Services.Interfaces;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;
        private readonly ILogger<ChatController> _logger;
        private readonly Pusher _pusher;
        public ChatController(
            IChatRoomService chatRoomService,
            IUserService userService,
            IStoreService storeService,
            IJwtTokenService jwtTokenService,
            Pusher pusher,
            ILogger<ChatController> logger)
        {
            _chatRoomService = chatRoomService;
            _storeService = storeService;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
            _pusher = pusher;
        }

        /// <summary>
        /// Lấy danh sách phòng chat của người dùng đang đăng nhập
        /// </summary>
        [HttpGet("my-chats")]
        public async Task<ActionResult<IEnumerable<ChatRoomResponseDTO>>> GetMyChats()
        {
            try
            {
                var userId = _jwtTokenService.GetUserIdFromToken(User);
                if (userId == null || userId <= 0)
                    return Unauthorized("Không tìm thấy thông tin người dùng");

                var filter = new ChatRoomCriteriaFilter
                {
                    UserId = userId,
                    PageSize = 50
                };

                var chatRooms = await _chatRoomService.Query(filter);
                chatRooms = chatRooms.Include(c => c.Buyer).Include(c => c.Seller);
                var results = await _chatRoomService.ApplyPagingAndSortingAsync(chatRooms, filter);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user chats");
                return StatusCode(500, "Có lỗi xảy ra khi lấy danh sách chat");
            }
        }

        /// <summary>
        /// Lấy danh sách tin nhắn trong phòng chat
        /// </summary>
        [HttpGet("room/{roomId}/messages")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChatMessages(int roomId, [FromQuery] int take = 50, [FromQuery] int skip = 0)
        {
            try
            {
                var userId = _jwtTokenService.GetUserIdFromToken(User);
                if (userId == null || userId <= 0)
                    return Unauthorized("Không tìm thấy thông tin người dùng");

                // Kiểm tra người dùng có quyền xem phòng chat này không
                var chatRoom = await _chatRoomService.GetByIdAsync(roomId);
                if (chatRoom == null)
                    return NotFound("Không tìm thấy phòng chat");

                if (chatRoom.BuyerId != userId && chatRoom.SellerId != userId && !User.IsInRole("Admin"))
                    return Forbid("Bạn không có quyền xem phòng chat này");

                var messages = await _chatRoomService.GetChatsByRoomIdAsync(roomId, take, skip);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat messages for room {RoomId}", roomId);
                return StatusCode(500, "Có lỗi xảy ra khi lấy tin nhắn");
            }
        }

        /// <summary>
        /// Gửi tin nhắn trong phòng chat
        /// </summary>
        [HttpPost("room/{roomId}/send")]
        public async Task<ActionResult> SendMessage(int roomId, [FromBody] string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                    return BadRequest("Nội dung tin nhắn không được trống");

                var userId = _jwtTokenService.GetUserIdFromToken(User);
                if (userId == null || userId <= 0)
                    return Unauthorized("Không tìm thấy thông tin người dùng");

                // Kiểm tra người dùng có quyền gửi tin nhắn trong phòng chat này không
                var chatRoom = await _chatRoomService.GetByIdAsync(roomId);
                if (chatRoom == null)
                    return NotFound("Không tìm thấy phòng chat");

                if (chatRoom.BuyerId != userId && chatRoom.SellerId != userId && !User.IsInRole("Admin"))
                    return Forbid("Bạn không có quyền gửi tin nhắn trong phòng chat này");

                // Nếu phòng chat đã đóng và người dùng không phải admin
                if (chatRoom.Status == ChatRoomStatus.Closed && !User.IsInRole("Admin"))
                    return BadRequest("Phòng chat đã đóng, không thể gửi tin nhắn mới");

                var chatDto = new ChatRequestDTO
                {
                    ChatRoomId = roomId,
                    SenderId = userId.Value,
                    Content = message
                };

                var success = await _chatRoomService.SendMessageAsync(chatDto);
                await _pusher.TriggerAsync(
                    channelName: roomId.ToString(),
                    eventName: "new-message",
                    data: new { message = message, roomId = roomId, senderId = userId.Value }
                );
                if (!success)
                    return StatusCode(500, "Không thể gửi tin nhắn");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to room {RoomId}", roomId);
                return StatusCode(500, "Có lỗi xảy ra khi gửi tin nhắn");
            }
        }

        [HttpGet("test-pusher")]
        [AllowAnonymous]
        public async Task<ActionResult> TestPusher([FromQuery] int userId, [FromQuery] string message, [FromQuery] int roomId)
        {
            ITriggerResult result = await _pusher.TriggerAsync(
                channelName: roomId.ToString(),
                eventName: "chat",
                data: new { message = message, roomId = roomId, senderId = userId }
            );
            _logger.LogInformation("Pusher triggered for room {RoomId}", roomId);
            _logger.LogInformation("Pusher result: {Result}", result.StatusCode);

            return Ok();
        }

        /// <summary>
        /// Tạo phòng chat mới giữa hai người dùng
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<ChatRoomResponseDTO>> CreateChatRoom([FromBody] CreateChatRoomRequestDTO request)
        {
            try
            {
                var currentUserId = _jwtTokenService.GetUserIdFromToken(User);
                if (currentUserId == null || currentUserId <= 0)
                    return Unauthorized("Không tìm thấy thông tin người dùng");

                // Kiểm tra người dùng đích tồn tại
                var targetStore = await _storeService.GetByIdAsync(request.TargetUserId);
                var targetUser = await _userService.GetByIdAsync(targetStore?.OwnerId ?? -1);
                if (targetStore == null || targetUser == null)
                    return NotFound("Không tìm thấy người dùng đích");

                request.TargetUserId = targetUser.Id;

                // Kiểm tra xem đã có phòng chat giữa hai người dùng chưa
                var filter = new ChatRoomCriteriaFilter
                {
                    UserId = currentUserId,
                    PageSize = 50
                };

                var chatRooms = await _chatRoomService.Query(filter);
                
                // Tìm phòng chat hiện có giữa người dùng hiện tại và người dùng đích
                var existingRoom = (await chatRooms
                    .FirstOrDefaultAsync(c => 
                        (c.BuyerId == currentUserId && c.SellerId == targetStore.OwnerId) || 
                        (c.SellerId == currentUserId && c.BuyerId == targetStore.OwnerId)));

                var existingRoomDTO = existingRoom?.ToResponseDTO();
                if (existingRoomDTO != null)
                {
                    _logger.LogInformation("Returning existing chat room between users {CurrentUser} and {TargetUser}", currentUserId, request.TargetUserId);
                    
                    // Nếu phòng chat đã đóng, mở lại
                    if (existingRoom.Status == ChatRoomStatus.Closed)
                    {
                        existingRoom.Status = ChatRoomStatus.InProgress;
                        await _chatRoomService.UpdateAsync(new ChatRoomRequestDTO
                        {
                            Id = existingRoom.Id,
                            BuyerId = existingRoom.BuyerId,
                            SellerId = existingRoom.SellerId,
                            Status = ChatRoomStatus.InProgress,
                        });
                    }
                    
                    // Include thông tin buyer và seller
                    existingRoomDTO.Buyer = await _userService.GetByIdAsync(existingRoom.BuyerId);
                    existingRoomDTO.Seller = await _userService.GetByIdAsync(existingRoom.SellerId);
                    
                    return Ok(existingRoom);
                }
                else{
                    // Nếu chưa có phòng chat, tạo mới
                    var chatRoom = await _chatRoomService.CreateChatRoomAsync(request);

                    if (chatRoom == null)
                        return StatusCode(500, "Không thể tạo phòng chat");

                    return Ok(chatRoom);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat room");
                return StatusCode(500, "Có lỗi xảy ra khi tạo phòng chat");
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một phòng chat
        /// </summary>
        [HttpGet("room/{id}")]
        public async Task<ActionResult<ChatRoomResponseDTO>> GetChatRoomDetails(int id)
        {
            try
            {
                var userId = _jwtTokenService.GetUserIdFromToken(User);
                if (userId == null || userId <= 0)
                    return Unauthorized("Không tìm thấy thông tin người dùng");

                var chatRoom = await _chatRoomService.GetByIdAsync(id);
                if (chatRoom == null)
                    return NotFound("Không tìm thấy phòng chat");

                // Kiểm tra quyền truy cập
                if (chatRoom.BuyerId != userId && chatRoom.SellerId != userId && !User.IsInRole("Admin"))
                    return Forbid("Bạn không có quyền xem phòng chat này");

                // Include buyer and seller
                chatRoom.Buyer = await _userService.GetByIdAsync(chatRoom.BuyerId);
                chatRoom.Seller = await _userService.GetByIdAsync(chatRoom.SellerId);

                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat room details for room {RoomId}", id);
                return StatusCode(500, "Có lỗi xảy ra khi lấy thông tin phòng chat");
            }
        }
    }

    
} 