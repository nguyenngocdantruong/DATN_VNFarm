using System;

namespace VNFarm.Infrastructure.External.Email
{
    public class EmailTemplates
    {
        public string GetWelcomeTemplate(string customerName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Chào mừng đến với VNFarm</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; padding: 10px; color: white; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Chào mừng đến với VNFarm</h2>
                    </div>
                    <div class='content'>
                        <p>Kính gửi {customerName},</p>
                        <p>Chúng tôi rất vui mừng khi bạn đã tham gia vào cộng đồng VNFarm - nền tảng kết nối người nông dân và người tiêu dùng hàng đầu Việt Nam.</p>
                        <p>Tại VNFarm, bạn có thể:</p>
                        <ul>
                            <li>Mua sắm sản phẩm nông nghiệp chất lượng cao trực tiếp từ người nông dân</li>
                            <li>Theo dõi nguồn gốc và quá trình sản xuất của sản phẩm</li>
                            <li>Hỗ trợ các hộ nông dân Việt Nam</li>
                        </ul>
                        <p>Hãy bắt đầu bằng cách khám phá các sản phẩm tươi ngon hoặc thiết lập cửa hàng của riêng bạn.</p>
                        <p>Thân ái,<br>Đội ngũ VNFarm</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} VNFarm. Tất cả các quyền được bảo lưu.</p>
                        <p>Địa chỉ: 123 Đường Nông Nghiệp, Quận 1, TP.HCM</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        }

        public string GetOrderConfirmationTemplate(string customerName, string orderNumber, decimal orderTotal)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Xác nhận đơn hàng #{orderNumber}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; padding: 10px; color: white; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .order-details {{ background-color: #f9f9f9; padding: 15px; margin: 15px 0; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Xác nhận đơn hàng</h2>
                    </div>
                    <div class='content'>
                        <p>Kính gửi {customerName},</p>
                        <p>Cảm ơn bạn đã đặt hàng tại VNFarm. Đơn hàng của bạn đã được xác nhận và đang được xử lý.</p>
                        <div class='order-details'>
                            <p><strong>Mã đơn hàng:</strong> #{orderNumber}</p>
                            <p><strong>Tổng tiền:</strong> {orderTotal:#,##0} VNĐ</p>
                            <p><strong>Ngày đặt hàng:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                        </div>
                        <p>Bạn có thể theo dõi trạng thái đơn hàng bằng cách đăng nhập vào tài khoản của mình trên trang web của chúng tôi.</p>
                        <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi.</p>
                        <p>Trân trọng,<br>Đội ngũ VNFarm</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} VNFarm. Tất cả các quyền được bảo lưu.</p>
                        <p>Địa chỉ: 123 Đường Nông Nghiệp, Quận 1, TP.HCM</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        }

        public string GetPasswordResetTemplate(string resetLink)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Đặt lại mật khẩu</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; padding: 10px; color: white; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .button {{ display: inline-block; padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px; margin: 15px 0; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Đặt lại mật khẩu</h2>
                    </div>
                    <div class='content'>
                        <p>Chào bạn,</p>
                        <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn tại VNFarm.</p>
                        <p>Để đặt lại mật khẩu, vui lòng nhấp vào nút bên dưới:</p>
                        <p style='text-align: center;'><a href='{resetLink}' class='button'>Đặt lại mật khẩu</a></p>
                        <p>Hoặc sao chép và dán liên kết này vào trình duyệt của bạn:</p>
                        <p>{resetLink}</p>
                        <p>Liên kết này sẽ hết hạn sau 24 giờ. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                        <p>Trân trọng,<br>Đội ngũ VNFarm</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} VNFarm. Tất cả các quyền được bảo lưu.</p>
                        <p>Địa chỉ: 123 Đường Nông Nghiệp, Quận 1, TP.HCM</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        }

        public string GetAccountVerificationTemplate(string verificationLink)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Xác thực tài khoản</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; padding: 10px; color: white; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .button {{ display: inline-block; padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px; margin: 15px 0; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Xác thực tài khoản</h2>
                    </div>
                    <div class='content'>
                        <p>Chào bạn,</p>
                        <p>Cảm ơn bạn đã đăng ký tài khoản tại VNFarm. Để hoàn tất quá trình đăng ký, vui lòng xác thực địa chỉ email của bạn bằng cách nhấp vào nút bên dưới:</p>
                        <p style='text-align: center;'><a href='{verificationLink}' class='button'>Xác thực tài khoản</a></p>
                        <p>Hoặc sao chép và dán liên kết này vào trình duyệt của bạn:</p>
                        <p>{verificationLink}</p>
                        <p>Liên kết này sẽ hết hạn sau 48 giờ.</p>
                        <p>Trân trọng,<br>Đội ngũ VNFarm</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} VNFarm. Tất cả các quyền được bảo lưu.</p>
                        <p>Địa chỉ: 123 Đường Nông Nghiệp, Quận 1, TP.HCM</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        }

        public string GetOrderStatusUpdateTemplate(string customerName, string orderNumber, string status)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Cập nhật trạng thái đơn hàng #{orderNumber}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; padding: 10px; color: white; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .order-details {{ background-color: #f9f9f9; padding: 15px; margin: 15px 0; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Cập nhật trạng thái đơn hàng</h2>
                    </div>
                    <div class='content'>
                        <p>Kính gửi {customerName},</p>
                        <p>Chúng tôi xin thông báo rằng trạng thái đơn hàng của bạn đã được cập nhật.</p>
                        <div class='order-details'>
                            <p><strong>Mã đơn hàng:</strong> #{orderNumber}</p>
                            <p><strong>Trạng thái mới:</strong> {status}</p>
                            <p><strong>Thời gian cập nhật:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                        </div>
                        <p>Bạn có thể theo dõi đơn hàng bằng cách đăng nhập vào tài khoản của mình trên trang web của chúng tôi.</p>
                        <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi.</p>
                        <p>Trân trọng,<br>Đội ngũ VNFarm</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} VNFarm. Tất cả các quyền được bảo lưu.</p>
                        <p>Địa chỉ: 123 Đường Nông Nghiệp, Quận 1, TP.HCM</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        }

        public string GetStoreApprovalTemplate(string storeName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Cửa hàng của bạn đã được phê duyệt</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; padding: 10px; color: white; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Chúc mừng! Cửa hàng của bạn đã được phê duyệt</h2>
                    </div>
                    <div class='content'>
                        <p>Chào bạn,</p>
                        <p>Chúng tôi vui mừng thông báo rằng cửa hàng <strong>{storeName}</strong> của bạn đã được phê duyệt và hiện đã hoạt động trên nền tảng VNFarm.</p>
                        <p>Bây giờ bạn có thể:</p>
                        <ul>
                            <li>Đăng sản phẩm để bán</li>
                            <li>Quản lý kho hàng của bạn</li>
                            <li>Thiết lập chính sách vận chuyển và thanh toán</li>
                            <li>Thiết lập các chương trình khuyến mãi</li>
                        </ul>
                        <p>Hãy đăng nhập vào tài khoản của bạn để bắt đầu.</p>
                        <p>Chúc bạn kinh doanh thuận lợi!</p>
                        <p>Trân trọng,<br>Đội ngũ VNFarm</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} VNFarm. Tất cả các quyền được bảo lưu.</p>
                        <p>Địa chỉ: 123 Đường Nông Nghiệp, Quận 1, TP.HCM</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        }

        public string GetStoreRejectionTemplate(string storeName, string reason)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Cửa hàng của bạn cần được cập nhật</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #FF6347; padding: 10px; color: white; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .reason {{ background-color: #f9f9f9; padding: 15px; margin: 15px 0; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Cửa hàng của bạn cần được cập nhật</h2>
                    </div>
                    <div class='content'>
                        <p>Chào bạn,</p>
                        <p>Chúng tôi đã xem xét cửa hàng <strong>{storeName}</strong> của bạn và hiện tại chúng tôi chưa thể phê duyệt nó với trạng thái hiện tại.</p>
                        <div class='reason'>
                            <p><strong>Lý do:</strong></p>
                            <p>{reason}</p>
                        </div>
                        <p>Bạn có thể cập nhật thông tin cửa hàng của mình và gửi lại yêu cầu xét duyệt. Chúng tôi sẽ xem xét lại yêu cầu của bạn trong thời gian sớm nhất.</p>
                        <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi để được hỗ trợ.</p>
                        <p>Trân trọng,<br>Đội ngũ VNFarm</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} VNFarm. Tất cả các quyền được bảo lưu.</p>
                        <p>Địa chỉ: 123 Đường Nông Nghiệp, Quận 1, TP.HCM</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        }
    }
} 