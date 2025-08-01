using YouAreHeard.Models.DTOs;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public AdminDashboardDTO GetDashboardSummary()
        {
            return new AdminDashboardDTO
            {
                TotalUsers = _adminRepository.GetTotalUserCount(),
                TotalDoctors = _adminRepository.GetTotalDoctorCount(),
                TotalPatients = _adminRepository.GetTotalPatientCount(),
                TotalAppointments = _adminRepository.GetTotalAppointmentCount(),
                ActiveTreatmentPlans = _adminRepository.GetActiveTreatmentPlanCount(),
                TotalLabResults = _adminRepository.GetTotalLabResultCount(),

                AverageDoctorRating = _adminRepository.GetAverageDoctorRating(),

                AppointmentStatusBreakdown = _adminRepository.GetAppointmentStatusBreakdown(),
                DoctorAppointmentLoad = _adminRepository.GetDoctorAppointmentLoad(),
                TopUsedMedications = _adminRepository.GetTopUsedMedications(),
                MostOrderedTestTypes = _adminRepository.GetMostOrderedTestTypes(),
                UserRoleDistribution = _adminRepository.GetUserRoleDistribution(),

                VerifiedOtpCount = _adminRepository.GetVerifiedOtpCount(),
                UnverifiedOtpCount = _adminRepository.GetUnverifiedOtpCount()
            };
        }
    }
}
