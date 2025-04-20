using System.Security.Cryptography;
using System.Text;

namespace VNFarm_FinalFinal.Helpers
{
    public static class AuthUtils
    {
        public static string GenerateMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
