using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Seller
{
    public class PaymentVoucherViewModel
    {
        public List<PaymentMethodResponseDTO> PaymentMethods { get; set; } = new();
        public List<DiscountResponseDTO> Discounts { get; set; } = new();
    }
}
