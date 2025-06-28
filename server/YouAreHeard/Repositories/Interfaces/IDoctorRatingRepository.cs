using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IDoctorRatingRepository
    {
        void AddRating(DoctorRatingDTO rating);
        void UpdateRating(DoctorRatingDTO rating);
        void DeleteRating(int doctorId, int userId);
        DoctorRatingDTO? GetRating(int doctorId, int userId);
        List<DoctorRatingDTO> GetRatingsByDoctor(int doctorId);
        double GetAverageRating(int doctorId);
    }
}