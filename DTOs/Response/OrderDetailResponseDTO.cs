using System;
using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace VNFarm_FinalFinal.DTOs.Response
{
    public class OrderDetailResponseDTO : BaseResponseDTO
    {
        public required int OrderId { get; set; }
        
        public required int ProductId { get; set; }
        
        public required int Quantity { get; set; }

        public required Unit Unit { get; set; }
        public required decimal UnitPrice { get; set; }
        
        public required decimal ShippingFee { get; set; }
        
        public required decimal TaxAmount { get; set; }
        
        public required decimal Subtotal { get; set; }

        public required OrderDetailStatus PackagingStatus { get; set; }
    }
}