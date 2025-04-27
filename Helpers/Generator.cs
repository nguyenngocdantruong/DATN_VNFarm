namespace VNFarm.Helpers
{
    public static class Generator
    {
        private static string GetRandom()
        {
            return new Guid().ToString("N");
        }
        public static string GenerateTransactionCode()
        {
            return $"#TS-" + GetRandom();
        }
        public static string GenerateOrderCode()
        {
            return $"#DH-" + GetRandom();
        }

        public static string GenerateBusinessRegistrationCode()
        {
            return $"#DKKD-" + GetRandom();
        }
    }
}
