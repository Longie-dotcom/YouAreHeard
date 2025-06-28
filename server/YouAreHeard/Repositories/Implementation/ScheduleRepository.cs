using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class ScheduleRepository : IScheduleRepository
    {
        public List<DoctorScheduleDTO> GetDoctorSchedules(int doctorId, int status, DateTime date)
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
                ds.doctorScheduleStatusID,
                dss.doctorScheduleStatusName
            FROM DoctorSchedule ds
            INNER JOIN DoctorScheduleStatus dss ON ds.doctorScheduleStatusID = dss.doctorScheduleStatusID
            WHERE ds.userID = @DoctorId 
              AND ds.doctorScheduleStatusID = @DoctorScheduleStatusID
              AND ds.date > @Date";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);
            command.Parameters.AddWithValue("@DoctorScheduleStatusID", status);
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
                    DoctorScheduleStatus = reader.GetInt32(reader.GetOrdinal("doctorScheduleStatusID")),
                    DoctorScheduleStatusName = reader.GetString(reader.GetOrdinal("doctorScheduleStatusName"))
                });
            }

            return schedules;
        }

        public List<DoctorScheduleDTO> GetAllDoctorSchedules(int doctorId, DateTime date)
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
                ds.doctorScheduleStatusID,
                dss.doctorScheduleStatusName
            FROM DoctorSchedule ds
            INNER JOIN DoctorScheduleStatus dss ON ds.doctorScheduleStatusID = dss.doctorScheduleStatusID
            WHERE ds.userID = @DoctorId 
              AND ds.date > @Date";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);
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
                    DoctorScheduleStatus = reader.GetInt32(reader.GetOrdinal("doctorScheduleStatusID")),
                    DoctorScheduleStatusName = reader.GetString(reader.GetOrdinal("doctorScheduleStatusName"))
                });
            }

            return schedules;
        }

        public DoctorScheduleDTO GetScheduleById(int scheduleId, int status, DateTime date)
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
                ds.doctorScheduleStatusID,
                dss.doctorScheduleStatusName
            FROM DoctorSchedule ds
            INNER JOIN DoctorScheduleStatus dss ON ds.doctorScheduleStatusID = dss.doctorScheduleStatusID
            WHERE ds.doctorScheduleID = @ScheduleId 
              AND ds.doctorScheduleStatusID = @DoctorScheduleStatusID
              AND ds.date > @Date";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ScheduleId", scheduleId);
            command.Parameters.AddWithValue("@DoctorScheduleStatusID", status);
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
                    DoctorScheduleStatus = reader.GetInt32(reader.GetOrdinal("doctorScheduleStatusID")),
                    DoctorScheduleStatusName = reader.GetString(reader.GetOrdinal("doctorScheduleStatusName"))
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
                ds.doctorScheduleStatusID,
                dss.doctorScheduleStatusName
            FROM DoctorSchedule ds
            INNER JOIN DoctorScheduleStatus dss ON ds.doctorScheduleStatusID = dss.doctorScheduleStatusID
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
                    DoctorScheduleStatus = reader.GetInt32(reader.GetOrdinal("doctorScheduleStatusID")),
                    DoctorScheduleStatusName = reader.GetString(reader.GetOrdinal("doctorScheduleStatusName"))
                };
            }

            return null;
        }

        public void UpdateScheduleStatus(int scheduleId, int status)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            var query = @"
                UPDATE DoctorSchedule
                SET doctorScheduleStatusID = @DoctorScheduleStatusID
                WHERE doctorScheduleID = @ScheduleId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorScheduleStatusID", status);
            command.Parameters.AddWithValue("@ScheduleId", scheduleId);

            command.ExecuteNonQuery();
        }

        public List<DoctorScheduleDTO> GetAllSchedulesWithAvailability(List<int> statusList, DateTime date)
        {
            List<DoctorScheduleDTO> schedules = new List<DoctorScheduleDTO>();

            using SqlConnection conn = DBContext.GetConnection();
            conn.Open();

            // Dynamically build parameterized IN clause
            var statusParams = statusList.Select((s, i) => $"@status{i}").ToList();
            string inClause = string.Join(", ", statusParams);

            string query = $@"
        SELECT 
            ds.DoctorScheduleID,
            ds.UserID,
            ds.Location,
            ds.StartTime,
            ds.EndTime,
            ds.Date,
            ds.doctorScheduleStatusID,
            dss.doctorScheduleStatusName,

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
        INNER JOIN DoctorScheduleStatus dss ON ds.doctorScheduleStatusID = dss.doctorScheduleStatusID
        WHERE ds.doctorScheduleStatusID IN ({inClause}) AND ds.Date > @Date
    ";

            using SqlCommand cmd = new SqlCommand(query, conn);

            // Add the status list as separate parameters
            for (int i = 0; i < statusList.Count; i++)
            {
                cmd.Parameters.AddWithValue($"@status{i}", statusList[i]);
            }

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
                    DoctorScheduleStatus = reader.GetInt32(reader.GetOrdinal("doctorScheduleStatusID")),
                    DoctorScheduleStatusName = reader.GetString(reader.GetOrdinal("doctorScheduleStatusName")),
                    DoctorProfile = doctorProfile
                });
            }

            return schedules;
        }

        public List<DoctorScheduleDTO> GetAllSchedules()
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
            ds.DoctorScheduleStatusID,
            dss.doctorScheduleStatusName,

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
        INNER JOIN DoctorScheduleStatus dss ON ds.doctorScheduleStatusID = dss.doctorScheduleStatusID
    ";

            using SqlCommand cmd = new SqlCommand(query, conn);

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
                    DoctorScheduleStatus = reader.GetInt32(reader.GetOrdinal("DoctorScheduleStatusID")),
                    DoctorScheduleStatusName = reader.GetString(reader.GetOrdinal("doctorScheduleStatusName")),
                    DoctorProfile = doctorProfile
                });
            }

            return schedules;
        }
    }
}