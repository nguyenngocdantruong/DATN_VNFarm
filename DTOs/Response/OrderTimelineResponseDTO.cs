using System;
using System.Collections.Generic;
using VNFarm.DTOs.Response;
using VNFarm.Enums;
using VNFarm.Helpers;

namespace VNFarm.DTOs.Response
{
    public class OrderTimelineResponseDTO: BaseResponseDTO
    {
        #region Thông tin cơ bản
        public required int OrderId { get; set; }
        public required OrderEventType EventType { get; set; } 
        public required OrderTimelineStatus Status { get; set; } 
        public required string Description { get; set; } 
        public string Icon
        {
            get => OrderUtils.GetIconForOrderTimeline(Status);
        }
        #endregion
    }
}
