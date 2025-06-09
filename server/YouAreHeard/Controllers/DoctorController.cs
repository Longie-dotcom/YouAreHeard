using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using YouAreHeard.Models;

namespace YouAreHeard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        [HttpGet("profile/{userId}")]
        public IActionResult GetDoctorProfile(int userId)
        {
            try
            {
                using (SqlConnection conn = DBContext.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT userID, specialties, yearsOfExperience, image, gender, description, languagesSpoken
                        FROM DoctorProfile
                        WHERE userID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return NotFound(new { message = "Không tìm thấy hồ sơ bác sĩ." });

                            var profile = new DoctorProfileDTO
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                                Specialties = reader["specialties"].ToString(),
                                YearsOfExperience = reader.GetInt32(reader.GetOrdinal("yearsOfExperience")),
                                Image = reader["image"].ToString(),
                                Gender = reader["gender"].ToString(),
                                Description = reader["description"].ToString(),
                                LanguagesSpoken = reader["languagesSpoken"].ToString()
                            };

                            return Ok(profile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ.", error = ex.Message });
            }
        }

        [HttpGet("schedule/{userId}")]
        public IActionResult GetDoctorSchedule(int userId)
        {
            try
            {
                using (SqlConnection conn = DBContext.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT ds.doctorScheduleID, ds.userID, ds.location, ds.startTime, ds.endTime,
                               ds.date, ds.isAvailable, ds.scheduleTypeID, st.scheduleTypeName
                        FROM DoctorSchedule ds
                        JOIN ScheduleType st ON ds.scheduleTypeID = st.scheduleTypeID
                        WHERE ds.userID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            var schedules = new List<DoctorScheduleDTO>();

                            while (reader.Read())
                            {
                                schedules.Add(new DoctorScheduleDTO
                                {
                                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                                    Location = reader["location"].ToString(),
                                    StartTime = (TimeSpan)reader["startTime"],
                                    EndTime = (TimeSpan)reader["endTime"],
                                    Date = (DateTime)reader["date"],
                                    IsAvailable = (bool)reader["isAvailable"],
                                    ScheduleTypeID = reader.GetInt32(reader.GetOrdinal("scheduleTypeID")),
                                    ScheduleTypeName = reader["scheduleTypeName"].ToString()
                                });
                            }

                            return Ok(schedules);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ.", error = ex.Message });
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllDoctors()
        {
            try
            {
                using (SqlConnection conn = DBContext.GetConnection())
                {
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
                                WHERE ds.userID = dp.userID AND ds.isAvailable = 1
                            ) AS d
                        ) AS availableDays
                    FROM DoctorProfile dp
                    INNER JOIN [User] u ON dp.userID = u.userID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
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

                            return Ok(doctors);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ.", error = ex.Message });
            }
        }
    }
}
