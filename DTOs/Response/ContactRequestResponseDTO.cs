namespace VNFarm.DTOs.Response
{
    public class ContactRequestResponseDTO : BaseResponseDTO
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string ServiceType { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Message { get; set; } = "";
    }
} 