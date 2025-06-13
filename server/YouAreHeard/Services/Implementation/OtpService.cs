using System.Net;
using System.Net.Mail;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;

        public OtpService(IOtpRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        public void GenerateAndSendOtp(string email)
        {
            var otp = GenerateOTP();
            SaveOtpToDatabase(email, otp);
            SendOtpEmail(email, otp);
        }

        public bool VerifyOtp(string email, string otp)
        {
            bool isValid = _otpRepository.IsOtpValid(email, otp);
            if (isValid)
            {
                _otpRepository.MarkOtpAsVerified(email, otp);
            }
            return isValid;
        }

        public void SaveOtpToDatabase(string email, string otp)
        {
            _otpRepository.InsertOrUpdateOtp(email, otp);
        }

        private string GenerateOTP(int length = 6)
        {
            var random = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(_ => random.Next(0, 10).ToString()));
        }

        private void SendOtpEmail(string toEmail, string otp)
        {
            var settings = EmailSettingsContext.Settings;

            var from = new MailAddress(settings.From, settings.DisplayName);
            var to = new MailAddress(toEmail);
            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            using var message = new MailMessage(from, to)
            {
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {otp}. It expires in 5 minutes."
            };

            smtp.Send(message);
        }
    }
}
