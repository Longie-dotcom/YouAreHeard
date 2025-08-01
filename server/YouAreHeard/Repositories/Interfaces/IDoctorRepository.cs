using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        DoctorProfileDTO GetDoctorProfileByDoctorId(int userId);
        List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId);
        List<DoctorProfileDTO> GetAllDoctorProfiles(int roleID);
        List<DoctorScheduleDTO> GetAllAvailableDoctorSchedules(int roleID);
        bool InsertDoctorProfile(DoctorProfileDTO doctor);
        List<DoctorProfileDTO> GetAllDoctorProfilesByAdmin();
    }
}