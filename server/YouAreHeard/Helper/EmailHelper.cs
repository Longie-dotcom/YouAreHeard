using System.Net;
using System.Net.Mail;

namespace YouAreHeard.Helper
{
    public static class EmailHelper
    {
        public static void SendOtpEmail(string toEmail, string otp)
        {
            var settings = EmailSettingsContext.Settings;

            var fromAddress = new MailAddress(settings.From, settings.DisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            string subject = "Mã xác minh OTP của bạn";

            string body = $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
              <meta charset='UTF-8' />
              <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
              <title>Xác minh Email</title>
            </head>
            <body style='margin: 0; padding: 0; font-family: Segoe UI, sans-serif; background: linear-gradient(160deg, #E3EBFA, #E7EDF7, #FFF7E8);'>

              <table width='100%' cellpadding='0' cellspacing='0' style='padding: 40px 0;'>
                <tr>
                  <td align='center'>
                    <table cellpadding='0' cellspacing='0' style='width: 100%; max-width: 520px; background-color: #ffffff; border-radius: 10px; padding: 40px 30px;'>

                      <tr>
                        <td align='center' style='padding-bottom: 20px;'>
                          <img src='https://youareheard.life/static/media/logo-picture.c3092cc0afe8dfffbe32.png' height='50' style='margin-right: 8px;' />
                          <img src='https://youareheard.life/static/media/logo-text.b175e6d3489102f17528.png' height='50' />
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 18px; color: #333; font-weight: 600; padding-bottom: 12px;'>
                          Xác minh Email
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 15px; color: #555; padding-bottom: 20px;'>
                          Vui lòng sử dụng mã dưới đây để xác minh địa chỉ email của bạn:
                        </td>
                      </tr>

                      <tr>
                        <td align='center'>
                          <div style='
                            font-size: 22px;
                            font-weight: 600;
                            color: #2a7ae2;
                            background: #f0f4ff;
                            padding: 10px 24px;
                            border-radius: 6px;
                            letter-spacing: 4px;
                            display: inline-block;
                            margin-bottom: 20px;
                          '>
                            {otp}
                          </div>
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 13px; color: #888;'>
                          Mã này sẽ hết hạn sau 5 phút. Nếu bạn không yêu cầu, hãy bỏ qua email này.
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 13px; color: #aaa; padding-top: 30px;'>
                          — Đội ngũ YouAreHeard —
                        </td>
                      </tr>

                    </table>
                  </td>
                </tr>
              </table>

            </body>
            </html>
            ";

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        public static void SendZoomLinkEmail(
            string toEmail,
            string doctorName,
            DateTime appointmentTime,
            TimeSpan startTime,
            string zoomLink,
            string passcode)
        {
            var settings = EmailSettingsContext.Settings;

            var fromAddress = new MailAddress(settings.From, settings.DisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            string subject = "Thông tin cuộc hẹn khám bệnh trực tuyến";

            string body = $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
              <meta charset='UTF-8' />
              <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
              <title>Cuộc hẹn khám bệnh</title>
            </head>
            <body style='margin: 0; padding: 0; font-family: Segoe UI, sans-serif; background: linear-gradient(160deg, #E3EBFA, #E7EDF7, #FFF7E8);'>

              <table width='100%' cellpadding='0' cellspacing='0' style='padding: 40px 0;'>
                <tr>
                  <td align='center'>
                    <table cellpadding='0' cellspacing='0' style='width: 100%; max-width: 520px; background-color: #ffffff; border-radius: 10px; padding: 40px 30px;'>

                      <!-- Logo -->
                      <tr>
                        <td align='center' style='padding-bottom: 20px;'>
                          <img src='https://youareheard.life/static/media/logo-picture.c3092cc0afe8dfffbe32.png' height='50' style='margin-right: 8px;' />
                          <img src='https://youareheard.life/static/media/logo-text.b175e6d3489102f17528.png' height='50' />
                        </td>
                      </tr>

                      <!-- Tiêu đề -->
                      <tr>
                        <td align='center' style='font-size: 18px; color: #333; font-weight: 600; padding-bottom: 12px;'>
                          Cuộc hẹn với Bác sĩ {doctorName}
                        </td>
                      </tr>

                      <!-- Nội dung -->
                      <tr>
                        <td align='center' style='font-size: 15px; color: #555; padding-bottom: 20px;'>
                          Dưới đây là thông tin chi tiết cuộc hẹn trực tuyến của bạn:
                        </td>
                      </tr>

                      <tr>
                        <td align='left' style='font-size: 15px; color: #333; padding-bottom: 20px; line-height: 1.6;'>
                          <strong>📅 Thời gian:</strong><br /> {appointmentTime:dddd, dd/MM/yyyy} {startTime}<br/><br/>
                          <strong>🔗 Đường dẫn Zoom:</strong><br /> <a href='{zoomLink}' style='color: #2a7ae2;'>{zoomLink}</a><br/><br/>
                          <strong>🔐 Mã truy cập:</strong><br /> {passcode}
                        </td>
                      </tr>

                      <!-- Lưu ý -->
                      <tr>
                        <td align='center' style='font-size: 14px; color: #888;'>
                          Vui lòng tham gia đúng giờ. Nếu bạn có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi.
                        </td>
                      </tr>

                      <!-- Ký tên -->
                      <tr>
                        <td align='center' style='font-size: 13px; color: #aaa; padding-top: 30px;'>
                          — Đội ngũ YouAreHeard —
                        </td>
                      </tr>

                    </table>
                  </td>
                </tr>
              </table>

            </body>
            </html>
            ";

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        public static void sendZoomLinkEmailToDoctor(string toEmail, string patientName, DateTime appointmentTime, TimeSpan startTime, string zoomLink, string passcode)
        {
            var settings = EmailSettingsContext.Settings;

            var fromAddress = new MailAddress(settings.From, settings.DisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            string subject = "Thông tin cuộc hẹn khám bệnh trực tuyến";

            string body = $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
              <meta charset='UTF-8' />
              <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
              <title>Cuộc hẹn khám bệnh</title>
            </head>
            <body style='margin: 0; padding: 0; font-family: Segoe UI, sans-serif; background: linear-gradient(160deg, #E3EBFA, #E7EDF7, #FFF7E8);'>

              <table width='100%' cellpadding='0' cellspacing='0' style='padding: 40px 0;'>
                <tr>
                  <td align='center'>
                    <table cellpadding='0' cellspacing='0' style='width: 100%; max-width: 520px; background-color: #ffffff; border-radius: 10px; padding: 40px 30px;'>

                      <!-- Logo -->
                      <tr>
                        <td align='center' style='padding-bottom: 20px;'>
                          <img src='https://youareheard.life/static/media/logo-picture.c3092cc0afe8dfffbe32.png' height='50' style='margin-right: 8px;' />
                          <img src='https://youareheard.life/static/media/logo-text.b175e6d3489102f17528.png' height='50' />
                        </td>
                      </tr>

                      <!-- Tiêu đề -->
                      <tr>
                        <td align='center' style='font-size: 18px; color: #333; font-weight: 600; padding-bottom: 12px;'>
                          Cuộc hẹn với Bệnh nhân {patientName}
                        </td>
                      </tr>

                      <!-- Nội dung -->
                      <tr>
                        <td align='center' style='font-size: 15px; color: #555; padding-bottom: 20px;'>
                          Dưới đây là thông tin chi tiết cuộc hẹn trực tuyến của bạn:
                        </td>
                      </tr>

                      <tr>
                        <td align='left' style='font-size: 15px; color: #333; padding-bottom: 20px; line-height: 1.6;'>
                          <strong>📅 Thời gian:</strong><br /> {appointmentTime:dddd, dd/MM/yyyy} {startTime}<br/><br/>
                          <strong>🔗 Đường dẫn Zoom:</strong><br /> <a href='{zoomLink}' style='color: #2a7ae2;'>{zoomLink}</a><br/><br/>
                          <strong>🔐 Mã truy cập:</strong><br /> {passcode}
                        </td>
                      </tr>

                      <!-- Lưu ý -->
                      <tr>
                        <td align='center' style='font-size: 14px; color: #888;'>
                          Vui lòng tham gia đúng giờ. Nếu bạn có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi. Cảm ơn các y bác sĩ đã tận tâm với công việc
                        </td>
                      </tr>

                      <!-- Ký tên -->
                      <tr>
                        <td align='center' style='font-size: 13px; color: #aaa; padding-top: 30px;'>
                          — Đội ngũ YouAreHeard —
                        </td>
                      </tr>

                    </table>
                  </td>
                </tr>
              </table>

            </body>
            </html>
            ";

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        public static void SendChangedAppointmentEmail(
            string toEmail,
            string doctorName,
            DateTime appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            string location)
        {
            var settings = EmailSettingsContext.Settings;

            var fromAddress = new MailAddress(settings.From, settings.DisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            string subject = "Cập nhật thông tin cuộc hẹn khám bệnh";

            string body = $@"
                <!DOCTYPE html>
                <html lang='vi'>
                <head>
                  <meta charset='UTF-8' />
                  <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
                  <title>Cập nhật cuộc hẹn</title>
                </head>
                <body style='margin: 0; padding: 0; font-family: Segoe UI, sans-serif; background: linear-gradient(160deg, #E3EBFA, #E7EDF7, #FFF7E8);'>

                  <table width='100%' cellpadding='0' cellspacing='0' style='padding: 40px 0;'>
                    <tr>
                      <td align='center'>
                        <table cellpadding='0' cellspacing='0' style='width: 100%; max-width: 520px; background-color: #ffffff; border-radius: 10px; padding: 40px 30px;'>

                          <!-- Logo -->
                          <tr>
                            <td align='center' style='padding-bottom: 20px;'>
                              <img src='https://youareheard.life/static/media/logo-picture.c3092cc0afe8dfffbe32.png' height='50' style='margin-right: 8px;' />
                              <img src='https://youareheard.life/static/media/logo-text.b175e6d3489102f17528.png' height='50' />
                            </td>
                          </tr>

                          <!-- Tiêu đề -->
                          <tr>
                            <td align='center' style='font-size: 18px; color: #333; font-weight: 600; padding-bottom: 12px;'>
                              Lịch hẹn khám của bạn đã được thay đổi
                            </td>
                          </tr>

                          <!-- Nội dung -->
                          <tr>
                            <td align='center' style='font-size: 15px; color: #555; padding-bottom: 20px;'>
                              Dưới đây là thông tin mới nhất của cuộc hẹn:
                            </td>
                          </tr>

                          <tr>
                            <td align='left' style='font-size: 15px; color: #333; padding-bottom: 20px; line-height: 1.6;'>
                              <strong>👨‍⚕️ Bác sĩ:</strong><br /> {doctorName}<br/><br/>
                              <strong>📅 Ngày khám:</strong><br /> {appointmentDate:dddd, dd/MM/yyyy}<br/><br/>
                              <strong>⏰ Thời gian:</strong><br /> {startTime} - {endTime}<br/><br/>
                              <strong>🏥 Địa điểm:</strong><br /> {location}
                            </td>
                          </tr>

                          <!-- Lưu ý -->
                          <tr>
                            <td align='center' style='font-size: 14px; color: #888;'>
                              Vui lòng đến đúng giờ. Nếu bạn có bất kỳ thắc mắc nào, đừng ngần ngại liên hệ với chúng tôi.
                            </td>
                          </tr>

                          <!-- Ký tên -->
                          <tr>
                            <td align='center' style='font-size: 13px; color: #aaa; padding-top: 30px;'>
                              — Đội ngũ YouAreHeard —
                            </td>
                          </tr>

                        </table>
                      </td>
                    </tr>
                  </table>

                </body>
                </html>
                ";

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        public static void SendQrCodeEmail(
        string toEmail,
        string doctorName,
        DateTime appointmentDate,
        TimeSpan startTime,
        string qrUrl,
        string orderCode)
        {
            var settings = EmailSettingsContext.Settings;

            var fromAddress = new MailAddress(settings.From, settings.DisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            string subject = "Mã QR xác nhận cuộc hẹn khám/điều trị trực tiếp";

            string body = $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
              <meta charset='UTF-8' />
              <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
              <title>Xác nhận cuộc hẹn</title>
            </head>
            <body style='margin: 0; padding: 0; font-family: Segoe UI, sans-serif; background: linear-gradient(160deg, #E3EBFA, #E7EDF7, #FFF7E8);'>

              <table width='100%' cellpadding='0' cellspacing='0' style='padding: 40px 0;'>
                <tr>
                  <td align='center'>
                    <table cellpadding='0' cellspacing='0' style='width: 100%; max-width: 520px; background-color: #ffffff; border-radius: 10px; padding: 40px 30px;'>

                      <tr>
                        <td align='center' style='padding-bottom: 20px;'>
                          <img src='https://youareheard.life/static/media/logo-picture.c3092cc0afe8dfffbe32.png' height='50' style='margin-right: 8px;' />
                          <img src='https://youareheard.life/static/media/logo-text.b175e6d3489102f17528.png' height='50' />
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 18px; color: #333; font-weight: 600; padding-bottom: 12px;'>
                          Cuộc hẹn với bác sĩ {doctorName}
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 15px; color: #555; padding-bottom: 20px;'>
                          Dưới đây là mã QR xác nhận cuộc hẹn của bạn:
                        </td>
                      </tr>

                      <tr>
                        <td align='center'>
                          <img src='{qrUrl}' alt='QR Code' style='width: 200px; height: 200px; margin-bottom: 20px;' />
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 15px; color: #333; padding-bottom: 20px;'>
                          <strong>📅 Ngày:</strong> {appointmentDate:dddd, dd/MM/yyyy}<br />
                          <strong>⏰ Giờ:</strong> {startTime}<br />
                          <strong>🔢 Mã định danh:</strong> {orderCode}
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 14px; color: #888;'>
                          Vui lòng mang theo mã QR này khi đến khám để xác nhận danh tính.
                        </td>
                      </tr>

                      <tr>
                        <td align='center' style='font-size: 13px; color: #aaa; padding-top: 30px;'>
                          — Đội ngũ YouAreHeard —
                        </td>
                      </tr>

                    </table>
                  </td>
                </tr>
              </table>

            </body>
            </html>";

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        public static async Task SendDoctorAccountEmailAsync(string toEmail, string password)
        {
            var settings = EmailSettingsContext.Settings;

            var fromAddress = new MailAddress(settings.From, settings.DisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            string subject = "Chào mừng đến với YouAreHeard - Tài khoản bác sĩ đã được tạo";
            string loginUrl = "https://youareheard.life/login";

            string htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
              <meta charset='UTF-8'>
              <title>Tài khoản bác sĩ được tạo</title>
              <style>
                body {{
                  font-family: Arial, sans-serif;
                  background-color: #f4f4f4;
                  margin: 0;
                  padding: 0;
                }}
                .container {{
                  max-width: 600px;
                  margin: 40px auto;
                  background-color: #ffffff;
                  border-radius: 8px;
                  padding: 30px;
                  box-shadow: 0 0 8px rgba(0,0,0,0.1);
                }}
                .header {{
                  text-align: center;
                  margin-bottom: 20px;
                }}
                .header h1 {{
                  color: #2d3748;
                }}
                .content {{
                  color: #4a5568;
                  font-size: 16px;
                  line-height: 1.6;
                }}
                .info-box {{
                  background-color: #edf2f7;
                  padding: 15px;
                  border-radius: 6px;
                  margin: 15px 0;
                }}
                .login-button {{
                  display: block;
                  width: fit-content;
                  margin: 20px auto;
                  padding: 12px 24px;
                  background-color: #2b6cb0;
                  color: #fff;
                  text-decoration: none;
                  border-radius: 6px;
                  font-weight: bold;
                }}
                .footer {{
                  text-align: center;
                  font-size: 13px;
                  color: #a0aec0;
                  margin-top: 30px;
                }}
              </style>
            </head>
            <body>
              <div class='container'>
                <div class='header'>
                  <h1>Chào mừng đến với YouAreHeard!</h1>
                </div>
                <div class='content'>
                  <p>Xin chào Bác sĩ,</p>
                  <p>Tài khoản của bác sĩ đã được quản trị viên tạo thành công. Dưới đây là thông tin đăng nhập của bác sĩ:</p>
                  <div class='info-box'>
                    <p><strong>Email:</strong> {toEmail}</p>
                    <p><strong>Mật khẩu:</strong> {password}</p>
                  </div>
                  <p>Bác sĩ có thể đăng nhập vào hệ thống bằng cách nhấn nút dưới đây:</p>
                  <a href='{loginUrl}' class='login-button'>Đăng nhập ngay</a>
                  <p>Nếu bác sĩ gặp khó khăn khi truy cập tài khoản, vui lòng liên hệ với đội ngũ hỗ trợ của chúng tôi.</p>
                  <p>Trân trọng cảm ơn bác sĩ đã tham gia vào hệ thống YouAreHeard.</p>
                </div>
                <div class='footer'>
                  &copy; 2025 YouAreHeard. Mọi quyền được bảo lưu.
                </div>
              </div>
            </body>
            </html>";

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}