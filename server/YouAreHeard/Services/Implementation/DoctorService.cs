using YouAreHeard.Enums;
using YouAreHeard.Models;
using YouAreHeard.NewFolder;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public DoctorService(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _scheduleRepository = scheduleRepository;
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

        public List<DoctorProfileDTO> GetAllDoctorProfiles()
        {
            return _doctorRepository.GetAllDoctorProfiles();
        }

        public List<DoctorScheduleDTO> GetAvailableDoctorSchedules()
        {
            CleanUpAllSchedulesAvailability();
            return _doctorRepository.GetAllAvailableDoctorSchedules();
        }

        private void CleanUpAndRefreshScheduleAvailability(DoctorScheduleDTO doctorSchedule)
        {
            // 1. Clean expired pending appointments
            _appointmentRepository.CancelExpiredPendingAppointmentsBySchedule(
                doctorSchedule.DoctorScheduleID,
                DateTime.Now,
                Constraints.ExpiredPendingAppointment
            );

            // 2. Count current queue (Confirmed + Pending)
            int currentQueue = _appointmentRepository.GetQueueCountByScheduleId(doctorSchedule.DoctorScheduleID, new List<int>
            {
                AppointmentStatusEnum.Confirmed,
                AppointmentStatusEnum.Pending
            });

            // 3. Determine if schedule should still be available
            int status = doctorSchedule.DoctorScheduleStatus;
            if (currentQueue == 0)
            {
                status = DoctorScheduleStatusEnum.Open;
            }
            else if (currentQueue < Constraints.AmountOfPersonPerSchedule)
            {
                status = DoctorScheduleStatusEnum.Pending;
            }
            ;


            // 4. Update availability in DB
            _scheduleRepository.UpdateScheduleStatus(doctorSchedule.DoctorScheduleID, status);
        }


        private void CleanUpAllSchedulesAvailability()
        {
            var allSchedules = _scheduleRepository.GetAllSchedules();
            foreach (var schedule in allSchedules)
            {
                CleanUpAndRefreshScheduleAvailability(schedule);
            }
        }
    }
}