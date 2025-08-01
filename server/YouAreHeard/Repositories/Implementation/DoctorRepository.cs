﻿using Microsoft.Data.SqlClient;
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
                    dp.languagesSpoken,

                    (
                        SELECT STRING_AGG(DATENAME(WEEKDAY, d.date), ', ')
                        FROM (
                            SELECT DISTINCT ds.date
                            FROM DoctorSchedule ds
                            WHERE ds.userID = dp.userID 
                        ) AS d
                    ) AS availableDays,

                    (
                        SELECT AVG(CAST(rateValue AS FLOAT)) 
                        FROM DoctorRating 
                        WHERE doctorID = dp.userID
                    ) AS AverageRating,

                    (
                        SELECT COUNT(*) 
                        FROM DoctorRating 
                        WHERE doctorID = dp.userID
                    ) AS TotalRatings

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

            return new DoctorProfileDTO
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
                AvailableDays = reader["availableDays"] != DBNull.Value ? reader["availableDays"].ToString() : "",
                AverageRating = reader["AverageRating"] != DBNull.Value ? Convert.ToDouble(reader["AverageRating"]) : (double?)null,
                TotalRatings = reader["TotalRatings"] != DBNull.Value ? Convert.ToInt32(reader["TotalRatings"]) : (int?)null
            };
        }

        public List<DoctorScheduleDTO> GetAllAvailableDoctorScheduleByDoctorId(int userId)
        {
            return _scheduleRepository.GetAllDoctorSchedules(userId, DateTime.Now);
        }

        public List<DoctorProfileDTO> GetAllDoctorProfiles(int roleID)
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
            ) AS availableDays,

            (
                SELECT AVG(CAST(rateValue AS FLOAT)) 
                FROM DoctorRating 
                WHERE doctorID = dp.userID
            ) AS AverageRating,

            (
                SELECT COUNT(*) 
                FROM DoctorRating 
                WHERE doctorID = dp.userID
            ) AS TotalRatings

        FROM DoctorProfile dp
        INNER JOIN [User] u ON dp.userID = u.userID
        WHERE u.roleID = @roleID";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@roleID", roleID); // Add parameter for roleID

            using var reader = cmd.ExecuteReader();

            var doctors = new List<DoctorProfileDTO>();
            while (reader.Read())
            {
                doctors.Add(new DoctorProfileDTO
                {
                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                    Specialties = reader["specialties"]?.ToString(),
                    YearsOfExperience = reader.GetInt32(reader.GetOrdinal("yearsOfExperience")),
                    Image = reader["image"]?.ToString(),
                    Gender = reader["gender"]?.ToString(),
                    Description = reader["description"]?.ToString(),
                    LanguagesSpoken = reader["languagesSpoken"]?.ToString(),
                    Name = reader["name"]?.ToString(),
                    Phone = reader["phone"]?.ToString(),
                    AvailableDays = reader["availableDays"] != DBNull.Value ? reader["availableDays"].ToString() : "",
                    AverageRating = reader["AverageRating"] != DBNull.Value ? Convert.ToDouble(reader["AverageRating"]) : (double?)null,
                    TotalRatings = reader["TotalRatings"] != DBNull.Value ? Convert.ToInt32(reader["TotalRatings"]) : (int?)null
                });
            }

            return doctors;
        }

        public List<DoctorProfileDTO> GetAllDoctorProfilesByAdmin()
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
            ) AS availableDays,

            (
                SELECT AVG(CAST(rateValue AS FLOAT)) 
                FROM DoctorRating 
                WHERE doctorID = dp.userID
            ) AS AverageRating,

            (
                SELECT COUNT(*) 
                FROM DoctorRating 
                WHERE doctorID = dp.userID
            ) AS TotalRatings

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
                    Specialties = reader["specialties"]?.ToString(),
                    YearsOfExperience = reader.GetInt32(reader.GetOrdinal("yearsOfExperience")),
                    Image = reader["image"]?.ToString(),
                    Gender = reader["gender"]?.ToString(),
                    Description = reader["description"]?.ToString(),
                    LanguagesSpoken = reader["languagesSpoken"]?.ToString(),
                    Name = reader["name"]?.ToString(),
                    Phone = reader["phone"]?.ToString(),
                    AvailableDays = reader["availableDays"] != DBNull.Value ? reader["availableDays"].ToString() : "",
                    AverageRating = reader["AverageRating"] != DBNull.Value ? Convert.ToDouble(reader["AverageRating"]) : (double?)null,
                    TotalRatings = reader["TotalRatings"] != DBNull.Value ? Convert.ToInt32(reader["TotalRatings"]) : (int?)null
                });
            }

            return doctors;
        }

        public List<DoctorScheduleDTO> GetAllAvailableDoctorSchedules(int roleID)
        {
            return _scheduleRepository.GetAllSchedulesWithAvailability(
                new List<int>() { DoctorScheduleStatusEnum.Open, DoctorScheduleStatusEnum.Pending, DoctorScheduleStatusEnum.Close },
                DateTime.Now,
                roleID
            );
        }

        public bool InsertDoctorProfile(DoctorProfileDTO doctor)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO DoctorProfile (UserID, Specialties, YearsOfExperience, Image, Gender, Description, LanguagesSpoken)
            VALUES (@UserID, @Specialties, @YearsOfExperience, @Image, @Gender, @Description, @LanguagesSpoken)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", doctor.UserID);
            cmd.Parameters.AddWithValue("@Specialties", doctor.Specialties);
            cmd.Parameters.AddWithValue("@YearsOfExperience", doctor.YearsOfExperience);
            cmd.Parameters.AddWithValue("@Image", doctor.Image ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", doctor.Gender ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", doctor.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LanguagesSpoken", doctor.LanguagesSpoken ?? (object)DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}