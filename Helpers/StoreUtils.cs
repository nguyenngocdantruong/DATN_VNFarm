namespace VNFarm.Helpers
{
    public class StoreUtils
    {
        public static Dictionary<int, string> GetStoreStatuses()
        {
            return new Dictionary<int, string>
            {
                { 0, "Chờ xác nhận" },
                { 1, "Đã xác nhận" },
                { -1, "Đã từ chối" }
            };
        }
        public static Dictionary<int, string> GetStoreTypes()
        {
            return new Dictionary<int, string>
            {
                { 0, "Hộ nông dân" },
                { 1, "Doanh nghiệp" }
            };
        }
    }
}
