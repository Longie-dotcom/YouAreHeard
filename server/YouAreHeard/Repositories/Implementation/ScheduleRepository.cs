using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class ScheduleRepository : IScheduleRepository
    {
        public List<DoctorScheduleDTO> GetDoctorSchedules(int doctorId, bool isAvailable, DateTime date)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            var query = @"
            SELECT 
                ds.doctorScheduleID,
                ds.userID,
                ds.location,
                ds.startTime,
                ds.endTime,
                ds.date,
                ds.isAvailable
            FROM DoctorSchedule ds
            WHERE ds.userID = @DoctorId 
              AND ds.isAvailable = @IsAvailable
              AND ds.date > @Date";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);
            command.Parameters.AddWithValue("@IsAvailable", isAvailable);
            command.Parameters.AddWithValue("@Date", date);

            using var reader = command.ExecuteReader();
            var schedules = new List<DoctorScheduleDTO>();

            while (reader.Read())
            {
                schedules.Add(new DoctorScheduleDTO
                {
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                    Location = reader.GetString(reader.GetOrdinal("location")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("startTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("endTime")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("isAvailable"))
                });
            }

            return schedules;
        }

        public DoctorScheduleDTO GetScheduleById(int scheduleId, bool isAvailable, DateTime date)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            var query = @"
            SELECT 
                ds.doctorScheduleID,
                ds.userID,
                ds.location,
                ds.startTime,
                ds.endTime,
                ds.date,
                ds.isAvailable
            FROM DoctorSchedule ds
            WHERE ds.doctorScheduleID = @ScheduleId 
              AND ds.isAvailable = @IsAvailable
              AND ds.date > @Date"; // Filter by date

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ScheduleId", scheduleId);
            command.Parameters.AddWithValue("@IsAvailable", isAvailable);
            command.Parameters.AddWithValue("@Date", date);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new DoctorScheduleDTO
                {
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                    Location = reader.GetString(reader.GetOrdinal("location")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("startTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("endTime")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("isAvailable"))
                };
            }

            return null;
        }

        public DoctorScheduleDTO GetScheduleById(int scheduleId, DateTime date)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            var query = @"
            SELECT 
                ds.doctorScheduleID,
                ds.userID,
                ds.location,
                ds.startTime,
                ds.endTime,
                ds.date,
                ds.isAvailable
            FROM DoctorSchedule ds
            WHERE ds.doctorScheduleID = @ScheduleId 
              AND ds.date > @Date";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ScheduleId", scheduleId);
            command.Parameters.AddWithValue("@Date", date);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new DoctorScheduleDTO
                {
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                    Location = reader.GetString(reader.GetOrdinal("location")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("startTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("endTime")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("isAvailable"))
                };
            }

            return null;
        }

        public void UpdateScheduleAvailability(int scheduleId, bool isAvailable)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            var query = @"
                UPDATE DoctorSchedule
                SET isAvailable = @IsAvailable
                WHERE doctorScheduleID = @ScheduleId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IsAvailable", isAvailable);
            command.Parameters.AddWithValue("@ScheduleId", scheduleId);

            command.ExecuteNonQuery();
        }

        public List<DoctorScheduleDTO> GetAllSchedules(bool isAvailable, DateTime date)
        {
            List<DoctorScheduleDTO> schedules = new List<DoctorScheduleDTO>();

            using SqlConnection conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    ds.DoctorScheduleID,
                    ds.UserID,
                    ds.Location,
                    ds.StartTime,
                    ds.EndTime,
                    ds.Date,
                    ds.IsAvailable

                    u.Name AS DoctorName,
                    u.Phone,

                    dp.Specialties,
                    dp.YearsOfExperience,
                    dp.Image,
                    dp.Gender,
                    dp.Description,
                    dp.LanguagesSpoken

                FROM DoctorSchedule ds
                INNER JOIN [User] u ON ds.UserID = u.UserID
                INNER JOIN DoctorProfile dp ON ds.UserID = dp.UserID
                WHERE ds.IsAvailable = @IsAvailable AND ds.Date > @Date
            ";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IsAvailable", isAvailable);
            cmd.Parameters.AddWithValue("@Date", date);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var doctorProfile = new DoctorProfileDTO
                {
                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                    Name = reader.GetString(reader.GetOrdinal("DoctorName")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    Specialties = reader.GetString(reader.GetOrdinal("Specialties")),
                    YearsOfExperience = reader.GetInt32(reader.GetOrdinal("YearsOfExperience")),
                    Image = reader.GetString(reader.GetOrdinal("Image")),
                    Gender = reader.GetString(reader.GetOrdinal("Gender")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    LanguagesSpoken = reader.GetString(reader.GetOrdinal("LanguagesSpoken")),
                };

                schedules.Add(new DoctorScheduleDTO
                {
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("DoctorScheduleID")),
                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                    Location = reader.GetString(reader.GetOrdinal("Location")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                    DoctorProfile = doctorProfile
                });
            }

            return schedules;
        }
    }
}
