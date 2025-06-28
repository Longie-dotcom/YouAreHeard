using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        DoctorProfileDTO GetDoctorProfileByDoctorId(int userId);
        List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId);
        List<DoctorProfileDTO> GetAllDoctorProfiles();
        List<DoctorScheduleDTO> GetAllAvailableDoctorSchedules();
    }
}