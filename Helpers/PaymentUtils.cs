using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Helpers
{
    public static class PaymentUtils
    {
        public static Dictionary<int, string> GetPaymentMethodsForForm()
        {
            return new Dictionary<int, string>
        {
            { (int)PaymentMethodEnum.BankTransfer, "Chuyển khoản" },
            { (int)PaymentMethodEnum.PaymentAfter, "Thanh toán sau" },
            { (int)PaymentMethodEnum.VNPay, "VNPay" }
        };
        }
    }
}
