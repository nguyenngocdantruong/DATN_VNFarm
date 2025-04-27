namespace VNFarm.Enums
{
    public enum StoreStatus
    {
        Pending = 0,
        Verified = 1,
        Rejected = -1,
        All = -999
    }
    public enum StoreType
    {
        All = -999,
        Farmer = 0, // Hộ nông dân
        Company = 1 // Doanh nghiệp
    }
}
