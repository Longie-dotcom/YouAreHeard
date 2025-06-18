using YouAreHeard.Models;
namespace YouAreHeard.Services.Interfaces
{
    public interface IZoomService
    {
        public Task<string> GenerateZoomLink(AppointmentDTO appointment);
    }
}