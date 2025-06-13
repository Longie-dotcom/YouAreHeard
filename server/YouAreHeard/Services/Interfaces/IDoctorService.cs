using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IDoctorService
    {
        DoctorProfileDTO GetDoctorProfileByDoctorId(string userId);
        List<DoctorScheduleDTO> GetDoctorScheduleByDoctorId(string userId);
        List<DoctorProfileDTO> GetAllDoctorProfiles();
    }
}
