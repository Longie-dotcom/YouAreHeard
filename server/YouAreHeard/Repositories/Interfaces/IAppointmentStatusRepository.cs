using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IAppointmentStatusRepository
    {
        List<AppointmentStatusDTO> GetAppointmentStatus();
    }
}
