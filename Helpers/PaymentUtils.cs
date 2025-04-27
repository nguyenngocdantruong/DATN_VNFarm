using VNFarm.Enums;

namespace VNFarm.Helpers
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
        public static Dictionary<int, string> GetPaymentStatusesForForm()
        {
            return new Dictionary<int, string>
            {
                { (int)PaymentStatus.Unpaid, "Chưa thanh toán" },
                { (int)PaymentStatus.PartiallyPaid, "Thanh toán một phần" },
                { (int)PaymentStatus.Paid, "Đã thanh toán" },
                { (int)PaymentStatus.Refunded, "Đã hoàn tiền" },
                { (int)PaymentStatus.Failed, "Thất bại" }
            };
        }
    }
}
