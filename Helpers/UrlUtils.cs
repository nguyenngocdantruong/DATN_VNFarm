namespace VNFarm_FinalFinal.Helpers
{
    public static class UrlUtils
    {
        public static string MakeUrl(string controller, string action, string id)
        {
            return $"/{controller}/{action}/{id}";
        }
    }
}
