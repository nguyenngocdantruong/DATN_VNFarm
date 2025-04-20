namespace VNFarm_FinalFinal.Helpers
{
    public static class TimeUtils
    {
        public static string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;
            if (timeSpan.Days > 365)
            {
                return $"{timeSpan.Days / 365} năm trước";
            }
            if (timeSpan.Days > 30)
            {
                return $"{timeSpan.Days / 30} tháng trước";
            }
            if (timeSpan.Days > 0)
            {
                return $"{timeSpan.Days} ngày trước";
            }
            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan.Hours} giờ trước";
            }
            if (timeSpan.Minutes > 0)
            {
                return $"{timeSpan.Minutes} phút trước";
            }
            return "Vừa xong";
        }
    }
}
