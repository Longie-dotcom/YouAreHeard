using Microsoft.Data.SqlClient;
using YouAreHeard.Enums;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IScheduleRepository _scheduleRepository;

        public DoctorRepository(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public DoctorProfileDTO GetDoctorProfileByDoctorId(int userId)
        {
            using SqlConnection conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT 
                dp.userID, 
                u.name AS doctorName, 
                u.phone,
                dp.specialties, 
                dp.yearsOfExperience, 
                dp.image, 
                dp.gender, 
                dp.description, 
                dp.languagesSpoken
            FROM DoctorProfile dp
            JOIN [User] u ON dp.userID = u.userID
            WHERE dp.userID = @UserID";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            var profile = new DoctorProfileDTO
            {
                UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                Name = reader["doctorName"]?.ToString(),
                Phone = reader["phone"]?.ToString(),
                Specialties = reader["specialties"]?.ToString(),
                YearsOfExperience = reader.GetInt32(reader.GetOrdinal("yearsOfExperience")),
                Image = reader["image"]?.ToString(),
                Gender = reader["gender"]?.ToString(),
                Description = reader["description"]?.ToString(),
                LanguagesSpoken = reader["languagesSpoken"]?.ToString(),
                AvailableDays = "" // You can compute this later if needed
            };

            return profile;
        }

        public List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId)
        {
            return _scheduleRepository.GetAllDoctorSchedules(userId, DateTime.Now);
        }

        public List<DoctorProfileDTO> GetAllDoctorProfiles()
        {
            using SqlConnection conn = DBContext.GetConnection();
            conn.Open();
            string query = @"
            SELECT 
            dp.userID, 
                        dp.specialties, 
                        dp.yearsOfExperience, 
                        dp.image, 
                        dp.gender, 
                        dp.description, 
                        dp.languagesSpoken,
                        u.name, 
                        u.phone,
                        (
                            SELECT STRING_AGG(DATENAME(WEEKDAY, d.date), ', ')
                            FROM (
                                SELECT DISTINCT ds.date
                                FROM DoctorSchedule ds
                                WHERE ds.userID = dp.userID 
                            ) AS d
                        ) AS availableDays
                    FROM DoctorProfile dp
                    INNER JOIN [User] u ON dp.userID = u.userID";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            var doctors = new List<DoctorProfileDTO>();
            while (reader.Read())
            {
                doctors.Add(new DoctorProfileDTO
                {
                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                    Specialties = reader["specialties"].ToString(),
                    YearsOfExperience = reader.GetInt32(reader.GetOrdinal("yearsOfExperience")),
                    Image = reader["image"].ToString(),
                    Gender = reader["gender"].ToString(),
                    Description = reader["description"].ToString(),
                    LanguagesSpoken = reader["languagesSpoken"].ToString(),
                    Name = reader["name"].ToString(),
                    Phone = reader["phone"].ToString(),
                    AvailableDays = reader["availableDays"] != DBNull.Value ? reader["availableDays"].ToString() : ""
                });
            }
            return doctors;
        }

        public List<DoctorScheduleDTO> GetAllAvailableDoctorSchedules()
        {
            return _scheduleRepository.GetAllSchedulesWithAvailability(
                new List<int>() { DoctorScheduleStatusEnum.Open, DoctorScheduleStatusEnum.Pending, DoctorScheduleStatusEnum.Close },
                DateTime.Now);
        }
    }
}