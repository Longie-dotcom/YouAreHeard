using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        DoctorScheduleDTO GetDoctorScheduleById(int id);
    }
}
