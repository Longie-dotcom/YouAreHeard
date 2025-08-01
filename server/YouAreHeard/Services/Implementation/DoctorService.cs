using YouAreHeard.Enums;
using YouAreHeard.Helper;
using YouAreHeard.Models;
using YouAreHeard.NewFolder;
using YouAreHeard.Repositories.Implementation;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IDoctorRatingRepository _doctorRatingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;

        public DoctorService(IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IScheduleRepository scheduleRepository,
            IDoctorRatingRepository doctorRatingRepository,
            IUserRepository userRepository,
            IOtpService otpService)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _scheduleRepository = scheduleRepository;
            _doctorRatingRepository = doctorRatingRepository;
            _userRepository = userRepository;
            _otpService = otpService;
        }

        public DoctorProfileDTO GetDoctorProfileByDoctorId(int userId)
        {
            return _doctorRepository.GetDoctorProfileByDoctorId(userId);
        }

        public List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId)
        {
            CleanUpAllSchedulesAvailability();
            return _doctorRepository.GetAllAvailableDoctorScheduleByDoctorId(userId);
        }

        public List<DoctorProfileDTO> GetAllDoctorProfiles(int roleID)
        {
            return _doctorRepository.GetAllDoctorProfiles(roleID);
        }

        public List<DoctorScheduleDTO> GetAvailableDoctorSchedules(int roleID)
        {
            CleanUpAllSchedulesAvailability();
            return _doctorRepository.GetAllAvailableDoctorSchedules(roleID);
        }

        private void CleanUpAndRefreshScheduleAvailability(DoctorScheduleDTO doctorSchedule)
        {
            // Clean expired pending appointments
            _appointmentRepository.CancelExpiredPendingAppointmentsBySchedule(
                doctorSchedule.DoctorScheduleID,
                DateTime.Now,
                Constraints.ExpiredPendingAppointment
            );

            // Count current queue (Confirmed + Pending)
            int currentQueue = _appointmentRepository.GetQueueCountByScheduleId(doctorSchedule.DoctorScheduleID, new List<int>
            {
                AppointmentStatusEnum.Confirmed,
                AppointmentStatusEnum.Pending
            });

            // Count current queue (Confirmed)
            int currentConfirmeds = _appointmentRepository.GetQueueCountByScheduleId(doctorSchedule.DoctorScheduleID, new List<int>
            {
                AppointmentStatusEnum.Confirmed
            });

            // Determine if schedule should still be available
            int status = doctorSchedule.DoctorScheduleStatus;
            if (currentQueue < Constraints.AmountOfPersonPerSchedule)
            {
                status = DoctorScheduleStatusEnum.Open;
            }
            else if (currentConfirmeds == Constraints.AmountOfPersonPerSchedule)
            {
                status = DoctorScheduleStatusEnum.Close;
            }
            else
            {
                status = DoctorScheduleStatusEnum.Pending;
            }


            // Update availability in DB
            _scheduleRepository.UpdateScheduleStatus(doctorSchedule.DoctorScheduleID, status);
        }

        public void CleanUpAllSchedulesAvailability()
        {
            var allSchedules = _scheduleRepository.GetAllSchedules();
            foreach (var schedule in allSchedules)
            {
                CleanUpAndRefreshScheduleAvailability(schedule);
            }
        }

        public void RatingDoctor(DoctorRatingDTO rating)
        {
            _doctorRatingRepository.AddRating(rating);
        }

        public async Task<bool> RegisterDoctor(UserDTO user, DoctorProfileDTO profile)
        {
            bool userInserted = _userRepository.InsertUser(user);
            if (!userInserted) return false;

            _otpService.GenerateAndAutoVerifyOtp(user.Email);

            var createdUser = _userRepository.GetUserByEmail(user.Email);
            if (createdUser == null) return false;

            profile.UserID = createdUser.UserId;

            await EmailHelper.SendDoctorAccountEmailAsync(createdUser.Email, createdUser.Password);

            return _doctorRepository.InsertDoctorProfile(profile);
        }

        public async Task SendDoctorAccountEmailsAsync(List<(string Email, string Password)> doctorAccounts)
        {
            foreach (var account in doctorAccounts)
            {
                await EmailHelper.SendDoctorAccountEmailAsync(account.Email, account.Password);
            }
        }

        public List<DoctorProfileDTO> GetAllDoctorProfilesByAdmin()
        {
            return _doctorRepository.GetAllDoctorProfilesByAdmin();
        }

        public void InsertSchedule(List<DoctorScheduleDTO> schedules)
        {
            foreach (var schedule in schedules)
            {
                schedule.DoctorScheduleID = DoctorScheduleStatusEnum.Open;
                _scheduleRepository.InsertSchedule(schedule);
            }
        }
    }
}