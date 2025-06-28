using YouAreHeard.Helper;
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
            EmailHelper.SendOtpEmail(toEmail, otp);
        }
    }
}
