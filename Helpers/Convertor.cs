namespace VNFarm.Helpers
{
    public static class Convertor
    {
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ToCustomDateTimeString(this DateTime dateTime)
        {
            string amPm = dateTime.Hour < 12 ? "sáng" : "chiều";
            return dateTime.ToString($"dd/MM/yyyy H:mm:ss {amPm}");
        }
        public static string ToOutputCurrency(this decimal amount)
        {
            return amount.ToString("N0") + " VNĐ";
        }
        public static string TimeFrameText(DateTime startDate, DateTime endDate)
        {
            return $"{startDate.ToCustomDateTimeString()} - {endDate.ToCustomDateTimeString()}";
        }
    }
}
