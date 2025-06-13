namespace YouAreHeard.Services.Interfaces
{
    public interface IEmailService
    {
        void SendOtpEmail(string toEmail, string otp);
        void SendZoomLinkEmail(
            string toEmail,
            string doctorName,
            DateTime appointmentTime,
            TimeSpan startTime,
            string zoomLink,
            string passcode);

        void sendZoomLinkEmailToDoctor(string toEmail, string patientName, DateTime appointmentTime, TimeSpan startTime, string zoomLink, string passcode);
    }


    
}
