namespace VNFarm.DTOs.Response
{
    public class TokenResultDTO
    {
        public string AccessToken { get; set; } = "";
        public DateTime Expiration { get; set; }
    }
}