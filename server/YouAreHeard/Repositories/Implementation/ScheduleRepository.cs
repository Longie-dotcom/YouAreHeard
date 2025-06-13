using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

public class ScheduleRepository : IScheduleRepository
{

    public DoctorScheduleDTO GetDoctorScheduleById(int id)
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
                    ds.isAvailable,
                    ds.scheduleTypeID,
                    st.scheduleTypeName
                FROM DoctorSchedule ds
                LEFT JOIN ScheduleType st ON ds.scheduleTypeID = st.scheduleTypeID
                WHERE ds.doctorScheduleID = @id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

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
                IsAvailable = reader.GetBoolean(reader.GetOrdinal("isAvailable")),
                ScheduleTypeID = reader.GetInt32(reader.GetOrdinal("scheduleTypeID")),
                ScheduleTypeName = reader["scheduleTypeName"]?.ToString()
            };
        }

        return null;
    }
}
