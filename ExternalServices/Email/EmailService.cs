using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using VNFarm_FinalFinal.Interfaces.External;

namespace VNFarm.Infrastructure.External.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly EmailTemplates _emailTemplates;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _emailTemplates = new EmailTemplates();

            _smtpServer = _configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            _senderEmail = _configuration["EmailSettings:SenderEmail"];
            _senderName = _configuration["EmailSettings:SenderName"];
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    client.EnableSsl = true;

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(_senderEmail, _senderName);
                        mailMessage.To.Add(to);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = isHtml;

                        await client.SendMailAsync(mailMessage);
                    }
                }

                _logger.LogInformation($"Email sent successfully to {to}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendWelcomeEmailAsync(string to, string customerName)
        {
            try
            {
                string subject = "Chào mừng đến với VNFarm";
                string body = _emailTemplates.GetWelcomeTemplate(customerName);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send welcome email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendOrderConfirmationEmailAsync(string to, string customerName, string orderNumber, decimal orderTotal)
        {
            try
            {
                string subject = $"Xác nhận đơn hàng #{orderNumber}";
                string body = _emailTemplates.GetOrderConfirmationTemplate(customerName, orderNumber, orderTotal);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send order confirmation email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string to, string resetLink)
        {
            try
            {
                string subject = "Đặt lại mật khẩu";
                string body = _emailTemplates.GetPasswordResetTemplate(resetLink);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send password reset email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendAccountVerificationEmailAsync(string to, string verificationLink)
        {
            try
            {
                string subject = "Xác thực tài khoản";
                string body = _emailTemplates.GetAccountVerificationTemplate(verificationLink);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send account verification email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendOrderStatusUpdateEmailAsync(string to, string customerName, string orderNumber, string status)
        {
            try
            {
                string subject = $"Cập nhật trạng thái đơn hàng #{orderNumber}";
                string body = _emailTemplates.GetOrderStatusUpdateTemplate(customerName, orderNumber, status);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send order status update email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendStoreApprovalEmailAsync(string to, string storeName)
        {
            try
            {
                string subject = "Cửa hàng của bạn đã được phê duyệt";
                string body = _emailTemplates.GetStoreApprovalTemplate(storeName);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send store approval email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendStoreRejectionEmailAsync(string to, string storeName, string reason)
        {
            try
            {
                string subject = "Cửa hàng của bạn cần được cập nhật";
                string body = _emailTemplates.GetStoreRejectionTemplate(storeName, reason);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send store rejection email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        // Các phương thức bổ sung theo yêu cầu của interface
        
        public async Task<bool> SendVerificationEmailAsync(string to, string verificationToken)
        {
            try
            {
                string subject = "Xác thực tài khoản VNFarm";
                string verificationLink = $"{_configuration["AppSettings:BaseUrl"]}/verify-account?token={verificationToken}";
                string body = _emailTemplates.GetAccountVerificationTemplate(verificationLink);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send verification email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendStoreVerificationResultAsync(string to, string storeName, bool isApproved, string reason)
        {
            try
            {
                if (isApproved)
                {
                    return await SendStoreApprovalEmailAsync(to, storeName);
                }
                else
                {
                    return await SendStoreRejectionEmailAsync(to, storeName, reason);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send store verification result email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendStoreStatusChangeAsync(string to, string storeName, bool isActive, string reason)
        {
            try
            {
                string subject = isActive ? 
                    $"Cửa hàng {storeName} đã được kích hoạt" : 
                    $"Cửa hàng {storeName} đã bị vô hiệu hóa";
                
                string template = isActive ? 
                    $"<p>Chào bạn,</p><p>Cửa hàng {storeName} của bạn đã được kích hoạt thành công trên hệ thống VNFarm.</p><p>Bạn có thể bắt đầu bán hàng ngay bây giờ.</p>" : 
                    $"<p>Chào bạn,</p><p>Cửa hàng {storeName} của bạn đã bị vô hiệu hóa vì lý do: {reason}.</p><p>Vui lòng liên hệ với quản trị viên để biết thêm chi tiết.</p>";
                
                return await SendEmailAsync(to, subject, template);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send store status change email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendOrderStatusUpdateAsync(string to, int orderId, string newStatus)
        {
            try
            {
                string subject = $"Cập nhật trạng thái đơn hàng #{orderId}";
                string body = $"<p>Chào bạn,</p><p>Đơn hàng #{orderId} của bạn đã được cập nhật trạng thái mới: <strong>{newStatus}</strong>.</p><p>Bạn có thể kiểm tra chi tiết đơn hàng trên trang cá nhân của mình.</p>";
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send order status update email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendBulkEmailAsync(List<string> recipients, string subject, string message)
        {
            try
            {
                // Phân chia danh sách người nhận thành các nhóm nhỏ để tránh quá tải SMTP server
                int batchSize = 50;
                int successCount = 0;
                
                for (int i = 0; i < recipients.Count; i += batchSize)
                {
                    var batch = recipients.Skip(i).Take(batchSize).ToList();
                    
                    using (var client = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        client.EnableSsl = true;

                        using (var mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(_senderEmail, _senderName);
                            mailMessage.Subject = subject;
                            mailMessage.Body = message;
                            mailMessage.IsBodyHtml = true;
                            
                            // Sử dụng Bcc để ẩn danh sách người nhận
                            foreach (var recipient in batch)
                            {
                                mailMessage.Bcc.Add(recipient);
                            }

                            await client.SendMailAsync(mailMessage);
                            successCount += batch.Count;
                        }
                    }
                }

                _logger.LogInformation($"Bulk email sent successfully to {successCount}/{recipients.Count} recipients");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send bulk email. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendDisputeNotificationAsync(string to, int disputeId, string disputeTitle)
        {
            try
            {
                string subject = $"Thông báo tranh chấp mới #{disputeId}";
                string body = $"<p>Chào bạn,</p><p>Có một tranh chấp mới được tạo với mã #{disputeId}:</p><p><strong>{disputeTitle}</strong></p><p>Vui lòng kiểm tra và phản hồi sớm nhất có thể.</p>";
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send dispute notification email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendNewDiscountNotificationAsync(string to, string discountCode, string discountDetails)
        {
            try
            {
                string subject = "Mã giảm giá mới cho bạn!";
                string body = $"<p>Chào bạn,</p><p>Chúng tôi vừa tạo một mã giảm giá đặc biệt dành cho bạn:</p><p><strong>{discountCode}</strong></p><p>Chi tiết: {discountDetails}</p><p>Hãy sử dụng mã này khi thanh toán để nhận ưu đãi.</p>";
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send discount notification email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendOrderConfirmationAsync(string to, int orderId, string orderDetails)
        {
            try
            {
                string subject = $"Xác nhận đơn hàng #{orderId}";
                string body = $"<p>Chào bạn,</p><p>Cảm ơn bạn đã đặt hàng tại VNFarm. Đơn hàng của bạn đã được xác nhận với mã #{orderId}.</p><p>Chi tiết đơn hàng:</p>{orderDetails}<p>Chúng tôi sẽ thông báo cho bạn khi đơn hàng được giao.</p>";
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send order confirmation email to {to}. Error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendPasswordResetAsync(string to, string resetToken)
        {
            try
            {
                string subject = "Đặt lại mật khẩu VNFarm";
                string resetLink = $"{_configuration["AppSettings:BaseUrl"]}/reset-password?token={resetToken}";
                string body = _emailTemplates.GetPasswordResetTemplate(resetLink);
                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send password reset email to {to}. Error: {ex.Message}");
                return false;
            }
        }
    }
} 