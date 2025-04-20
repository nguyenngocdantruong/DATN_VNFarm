using System;
using System.Collections.Generic;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Helpers;

namespace VNFarm_FinalFinal.DTOs.Response
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
