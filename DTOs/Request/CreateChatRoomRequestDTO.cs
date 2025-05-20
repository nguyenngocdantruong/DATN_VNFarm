using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class CreateChatRoomRequestDTO
    {
        [Required]
        public int TargetUserId { get; set; }
        [Required]
        public int CurrentUserId { get; set; }
        [Required]
        public string RoomName { get; set; } = "default";
        [Required]
        public string Description { get; set; } = "";
    }
}