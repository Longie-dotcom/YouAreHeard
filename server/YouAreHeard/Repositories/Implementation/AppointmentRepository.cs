using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using YouAreHeard.Enums;
using YouAreHeard.NewFolder;

namespace YouAreHeard.Repositories.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public void RequestAppointment(AppointmentDTO appointment, MedicalHistoryDTO medicalHistory)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Insert MedicalHistory
                string insertMedicalHistoryQuery = @"
                    INSERT INTO MedicalHistory (dateTime, patientID, doctorID)
                    OUTPUT INSERTED.medicalHistoryID
                    VALUES (@DateTime, @PatientID, @DoctorID)";

                using var mhCmd = new SqlCommand(insertMedicalHistoryQuery, conn, transaction);
                mhCmd.Parameters.AddWithValue("@DateTime", medicalHistory.DateTime);
                mhCmd.Parameters.AddWithValue("@PatientID", medicalHistory.PatientID);
                mhCmd.Parameters.AddWithValue("@DoctorID", medicalHistory.DoctorID);

                int medicalHistoryID = (int)mhCmd.ExecuteScalar();

                // Insert Appointment
                string insertAppointmentQuery = @"
                    INSERT INTO Appointment 
                    (medicalHistoryID, doctorScheduleID, appointmentStatusID, zoomLink, notes, reason, isAnonymous)
                    VALUES 
                    (@MedicalHistoryID, @DoctorScheduleID, @AppointmentStatusID, @ZoomLink, @Notes, @Reason, @IsAnonymous)";

                using var aCmd = new SqlCommand(insertAppointmentQuery, conn, transaction);
                aCmd.Parameters.AddWithValue("@MedicalHistoryID", medicalHistoryID);
                aCmd.Parameters.AddWithValue("@DoctorScheduleID", appointment.DoctorScheduleID);
                aCmd.Parameters.AddWithValue("@AppointmentStatusID", AppointmentStatusEnum.Confirmed);
                aCmd.Parameters.AddWithValue("@ZoomLink", appointment.ZoomLink ?? (object)DBNull.Value);
                aCmd.Parameters.AddWithValue("@Notes", appointment.Notes ?? (object)DBNull.Value);
                aCmd.Parameters.AddWithValue("@Reason", appointment.Reason ?? (object)DBNull.Value);
                aCmd.Parameters.AddWithValue("@IsAnonymous", appointment.IsAnonymous);

                aCmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    a.appointmentID,
                    a.medicalHistoryID,
                    a.doctorScheduleID,
                    a.zoomLink,
                    a.notes,
                    a.reason,
                    a.isAnonymous,
                    a.appointmentStatusID,
                    s.appointmentStatusName
                FROM Appointment a
                INNER JOIN MedicalHistory m ON a.medicalHistoryID = m.medicalHistoryID
                INNER JOIN AppointmentStatus s ON a.appointmentStatusID = s.appointmentStatusID
                WHERE m.patientID = @PatientID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", patientId);
            using var reader = cmd.ExecuteReader();

            var appointments = new List<AppointmentDTO>();

            while (reader.Read())
            {
                appointments.Add(new AppointmentDTO
                {
                    AppointmentID = reader.GetInt32(reader.GetOrdinal("appointmentID")),
                    MedicalHistoryID = reader.GetInt32(reader.GetOrdinal("medicalHistoryID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    ZoomLink = reader["zoomLink"]?.ToString(),
                    Notes = reader["notes"]?.ToString(),
                    Reason = reader["reason"]?.ToString(),
                    IsAnonymous = reader.GetBoolean(reader.GetOrdinal("isAnonymous")),
                    AppointmentStatusID = reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    AppointmentStatusName = reader["appointmentStatusName"]?.ToString()
                });
            }

            return appointments;
        }
    }
}
