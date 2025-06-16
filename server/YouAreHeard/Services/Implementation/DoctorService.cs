using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public DoctorProfileDTO GetDoctorProfileByDoctorId(int userId)
        {
            return _doctorRepository.GetDoctorProfileByDoctorId(userId);
        }

        public List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId)
        {
            return _doctorRepository.GetAllAvailableDoctorScheduleByDoctorId(userId);
        }

        public List<DoctorProfileDTO> GetAllDoctorProfiles()
        {
            return _doctorRepository.GetAllDoctorProfiles();
        }

        public List<DoctorScheduleDTO> GetAvailableDoctorSchedules()
        {
            return _doctorRepository.GetAllAvailableDoctorSchedules();
        }
    }
}
