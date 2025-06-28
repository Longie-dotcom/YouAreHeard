using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IDoctorService
    {
        DoctorProfileDTO GetDoctorProfileByDoctorId(int userId);
        List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId);
        List<DoctorProfileDTO> GetAllDoctorProfiles();
        List<DoctorScheduleDTO> GetAvailableDoctorSchedules();
        void RatingDoctor(DoctorRatingDTO rating);
    }
}