using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class NotificationResponseDTO : BaseResponseDTO
    {
        public required int UserId { get; set; }
        public required string Content { get; set; }
        public string? LinkUrl { get; set; }
        public required NotificationType Type { get; set; }
        public required bool IsRead { get; set; }
    }
}