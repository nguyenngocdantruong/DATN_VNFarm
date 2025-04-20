using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.ViewModels.Seller
{
    public class PaymentVoucherViewModel
    {
        public List<PaymentMethodResponseDTO> PaymentMethods { get; set; } = new();
        public List<DiscountResponseDTO> Discounts { get; set; } = new();
    }
}
