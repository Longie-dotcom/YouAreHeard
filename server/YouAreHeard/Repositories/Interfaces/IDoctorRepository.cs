using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        DoctorProfileDTO GetDoctorProfileByDoctorId(int userId);
        List<DoctorScheduleDTO> GetDoctorScheduleByDoctorId(string userId);
        List<DoctorProfileDTO> GetAllDoctorProfiles();
    }
}
