using System.Net;
using System.Net.Mail;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
  public class EmailService : IEmailService
  {
    public void SendOtpEmail(string toEmail, string otp)
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


    public void SendZoomLinkEmail(
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

    public void sendZoomLinkEmailToDoctor(string toEmail, string patientName, DateTime appointmentTime, TimeSpan startTime, string zoomLink, string passcode)
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

  }
}
