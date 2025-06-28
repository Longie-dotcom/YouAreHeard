using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        List<DoctorScheduleDTO> GetDoctorSchedules(int doctorId, int status, DateTime date);
        List<DoctorScheduleDTO> GetAllDoctorSchedules(int doctorId, DateTime date);
        DoctorScheduleDTO GetScheduleById(int scheduleId, int status, DateTime date);
        DoctorScheduleDTO GetScheduleById(int scheduleId, DateTime date);
        public void UpdateScheduleStatus(int scheduleId, int status);
        public List<DoctorScheduleDTO> GetAllSchedulesWithAvailability(List<int> statusList, DateTime date);
        List<DoctorScheduleDTO> GetAllSchedules();
    }
}