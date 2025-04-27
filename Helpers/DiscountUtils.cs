using VNFarm.Enums;

namespace VNFarm.Helpers
{
    public static class DiscountUtils
    {
        public static decimal CalculateDiscount(decimal originalPrice, decimal discountAmount, DiscountType discountType)
        {
            switch (discountType)
            {
                case DiscountType.Percentage:
                    return originalPrice * (1 - discountAmount / 100);
                case DiscountType.FixedAmount:
                    return originalPrice - discountAmount;
                case DiscountType.FreeShipping:
                    return originalPrice;
                default:
                    return originalPrice;
            }  
        }
        private static Dictionary<DiscountType, string> DiscountTypeNames = new Dictionary<DiscountType, string>
        {
            {DiscountType.Percentage, "Phần trăm"},
            {DiscountType.FixedAmount, "Số tiền cố định"},
            {DiscountType.FreeShipping, "Miễn phí vận chuyển"}
        };
        private static Dictionary<DiscountStatus, string> DiscountStatusNames = new Dictionary<DiscountStatus, string>
        {
            {DiscountStatus.Active, "Hoạt động"},
            {DiscountStatus.Inactive, "Không hoạt động"}
        };
        public static Dictionary<int, string> GetDiscountsTypeForForm()
        {
            return DiscountTypeNames.ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Value);
        }
        public static Dictionary<int, string> GetDiscountStatusForForm()
        {
            return DiscountStatusNames.ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Value);
        }
    }
}
