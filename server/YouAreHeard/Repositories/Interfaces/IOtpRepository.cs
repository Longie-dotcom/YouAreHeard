namespace YouAreHeard.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        bool OtpExistsAndUnverified(string email);
        void InsertOrUpdateOtp(string email, string otp);
        bool IsOtpValid(string email, string otp);
        void MarkOtpAsVerified(string email, string otp);
    }
}