namespace VNFarm.Helpers
{
    public static class FileUpload
    {
        public static string ReviewFolder = "Reviews";
        public static string BusinessLicenseFolder = "BusinessLicenses";
        public static string CategoryFolder = "Categories";
        public static string ProductFolder = "Products";
        public static string StoreFolder = "Stores";
        public static string UserFolder = "Users";
        private static string[] AllowedExtensions = { ".pdf", ".docx", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        private static long MaxFileSize = 10 * 1024 * 1024; // 10MB
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            // Check if file is null or empty
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Không có file được tải lên");
            }
            // Check if file size is too large
            if (file.Length > MaxFileSize)
            {
                throw new ArgumentException("Kích thước file vượt quá 10MB");
            }
            // Check if file extension is allowed
            var fileExtension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Định dạng file không hợp lệ");
            }
            // Generate unique file name
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        public static bool IsAllowedExtension(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            return !string.IsNullOrEmpty(fileName) || AllowedExtensions.Contains(fileExtension.ToLower());
        }
    }
}