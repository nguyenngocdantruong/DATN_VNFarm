namespace VNFarm.Enums
{
    public enum PaymentStatus
    {
        All = -999,
        Unpaid = 1,
        PartiallyPaid = 2,
        Paid = 3,
        Refunded = 4,
        Failed = 5
    }
    public enum PaymentType
    {
        All = -999,
        Bank = 1,
        Visa = 2,
        Mastercard = 3,
        ZaloPay = 4,
        Momo = 5
    }

} 