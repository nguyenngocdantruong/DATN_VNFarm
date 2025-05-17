using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class VoucherResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public decimal Value { get; set; }
        public DiscountType Type { get; set; }
    }
} 