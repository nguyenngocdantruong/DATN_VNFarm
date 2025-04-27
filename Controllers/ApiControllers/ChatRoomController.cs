using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Services;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomController : ApiBaseController<ChatRoom, ChatRoomRequestDTO, ChatRoomResponseDTO>
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatRoomController(IChatRoomService chatRoomService, ILogger<ChatRoomController> logger) : base(chatRoomService, logger)
        {
            _chatRoomService = chatRoomService;
        }

        /// <summary>
        /// Lấy danh sách tin nhắn trong phòng chat
        /// </summary>
        [HttpGet("{roomId}/chats")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChatsByRoomId(int roomId, [FromQuery] int take = 20, [FromQuery] int skip = 0)
        {
            var chats = await _chatRoomService.GetChatsByRoomIdAsync(roomId, take, skip);
            return Ok(chats);
        }

        /// <summary>
        /// Lấy danh sách phòng chat của người dùng
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ChatRoomResponseDTO>>> GetUserChatList(int userId)
        {
            var chatRooms = await _chatRoomService.GetUserChatListAsync(userId);
            return Ok(chatRooms);
        }

        /// <summary>
        /// Gửi tin nhắn trong phòng chat
        /// </summary>
        [HttpPost("send")]
        public async Task<ActionResult> SendMessage([FromBody] ChatRequestDTO chatDTO)
        {
            var success = await _chatRoomService.SendMessageAsync(chatDTO);
            if (!success)
                return BadRequest("Không thể gửi tin nhắn");

            return Ok();
        }

        /// <summary>
        /// Lấy danh sách phòng chat theo bộ lọc
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<ChatRoomResponseDTO>>> GetChatRoomsByFilter([FromBody] ChatRoomCriteriaFilter filter)
        {
            var chatRooms = await _chatRoomService.Query(filter);
            var results = await _chatRoomService.ApplyPagingAndSortingAsync(chatRooms, filter);
            return Ok(results);
        }
    }
}

