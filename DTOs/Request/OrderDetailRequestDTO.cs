using System;
using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace VNFarm_FinalFinal.DTOs.Request
{
    public class OrderDetailRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Mã đơn hàng không được để trống")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
}