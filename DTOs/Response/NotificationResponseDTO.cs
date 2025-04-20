using System;
using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace VNFarm_FinalFinal.DTOs.Response
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