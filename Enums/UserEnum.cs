namespace VNFarm.Enums
{
    public enum UserRole
    {
        Admin = 1,
        Seller = 2,
        Buyer = 3,
        User = -1,
        All = -999
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive = 0,
        Blocked = 2,
        All = -999
    }
} 