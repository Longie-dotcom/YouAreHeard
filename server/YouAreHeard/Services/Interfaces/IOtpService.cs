namespace YouAreHeard.Services.Interfaces
{
    public interface IOtpService
    {
        void GenerateAndSendOtp(string email);
        bool VerifyOtp(string email, string otp);
        void SaveOtpToDatabase(string email, string otp);
        void GenerateAndAutoVerifyOtp(string email);
    }
}
