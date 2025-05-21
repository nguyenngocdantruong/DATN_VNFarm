using Microsoft.Extensions.Caching.Memory;

namespace VNFarm.Caching
{
    public class MyOtpService(IMemoryCache memoryCache) 
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        public void SetOtp(string email, int value)
        {
            _memoryCache.Set(email, value, TimeSpan.FromMinutes(15));
        }

        public int GetOtp(string email)
        {
            if (_memoryCache.TryGetValue(email, out int value))
            {
                return value;
            }

            return -1;
        }
    }
}
