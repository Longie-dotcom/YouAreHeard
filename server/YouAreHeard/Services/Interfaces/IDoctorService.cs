using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IDoctorService
    {
        DoctorProfileDTO GetDoctorProfileByDoctorId(int userId);
        List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId);
        List<DoctorProfileDTO> GetAllDoctorProfiles(int roleID);
        List<DoctorScheduleDTO> GetAvailableDoctorSchedules(int roleID);
        Task<bool> RegisterDoctor(UserDTO user, DoctorProfileDTO profile);
        void RatingDoctor(DoctorRatingDTO rating);
        void CleanUpAllSchedulesAvailability();
        Task SendDoctorAccountEmailsAsync(List<(string Email, string Password)> doctorAccounts);
        List<DoctorProfileDTO> GetAllDoctorProfilesByAdmin();
        void InsertSchedule(List<DoctorScheduleDTO> schedule);
    }
}