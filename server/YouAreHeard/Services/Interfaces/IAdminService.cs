using YouAreHeard.Models.DTOs;

namespace YouAreHeard.Services.Interfaces
{
    public interface IAdminService
    {
        AdminDashboardDTO GetDashboardSummary();
    }
}
