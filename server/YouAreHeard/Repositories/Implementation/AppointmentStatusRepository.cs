using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class AppointmentStatusRepository : IAppointmentStatusRepository
    {
        public List<AppointmentStatusDTO> GetAppointmentStatus()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"SELECT * FROM AppointmentStatus";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var statuses = new List<AppointmentStatusDTO>();

            while (reader.Read())
            {
                AppointmentStatusDTO status = new AppointmentStatusDTO()
                {
                    AppointmentStatusID = reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    AppointmentStatusName = reader["appointmentStatusName"]?.ToString()
                };
                statuses.Add(status);
            }
            return statuses;
        }
    }
}
