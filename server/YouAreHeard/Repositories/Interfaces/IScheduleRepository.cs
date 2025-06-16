using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        List<DoctorScheduleDTO> GetDoctorSchedules(int doctorId, bool isAvailable, DateTime date);
        DoctorScheduleDTO GetScheduleById(int scheduleId, bool isAvailable, DateTime date);
        DoctorScheduleDTO GetScheduleById(int scheduleId, DateTime date);
        public void UpdateScheduleAvailability(int scheduleId, bool isAvailable);
        List<DoctorScheduleDTO> GetAllSchedules(bool isAvailable, DateTime date);
    }
}
