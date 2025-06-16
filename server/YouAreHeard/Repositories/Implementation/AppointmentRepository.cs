using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using YouAreHeard.NewFolder;

namespace YouAreHeard.Repositories.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public int InsertAppointment(AppointmentDTO appointment)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO Appointment 
            (medicalHistoryID, doctorScheduleID, appointmentStatusID, zoomLink, notes, reason, isAnonymous, queueNumber)
            OUTPUT INSERTED.appointmentID
            VALUES 
            (@MedicalHistoryID, @DoctorScheduleID, @AppointmentStatusID, @ZoomLink, @Notes, @Reason, @IsAnonymous, @QueueNumber)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MedicalHistoryID", appointment.MedicalHistoryID);
            cmd.Parameters.AddWithValue("@DoctorScheduleID", appointment.DoctorScheduleID);
            cmd.Parameters.AddWithValue("@AppointmentStatusID", appointment.AppointmentStatusID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ZoomLink", appointment.ZoomLink ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", appointment.Notes ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Reason", appointment.Reason ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IsAnonymous", appointment.IsAnonymous);
            cmd.Parameters.AddWithValue("@QueueNumber", appointment.QueueNumber ?? (object)DBNull.Value);

            // Execute and return the inserted ID
            return (int)cmd.ExecuteScalar();
        }

        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId, int statusId)
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
                a.queueNumber,
                s.appointmentStatusName,
                u.name AS doctorName,
                ds.date,
                ds.startTime,
                ds.endTime,
                ds.location,
                p.name AS patientName,
                m.patientID,
                m.doctorID
            FROM Appointment a
            INNER JOIN MedicalHistory m ON a.medicalHistoryID = m.medicalHistoryID
            INNER JOIN AppointmentStatus s ON a.appointmentStatusID = s.appointmentStatusID
            INNER JOIN DoctorSchedule ds ON a.doctorScheduleID = ds.doctorScheduleID
            INNER JOIN [User] u ON m.doctorID = u.userID
            INNER JOIN [User] p ON m.patientID = p.userID
            WHERE m.patientID = @PatientID AND a.appointmentStatusID = @StatusID
            ORDER BY ds.date ASC, ds.startTime ASC";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", patientId);
            cmd.Parameters.AddWithValue("@StatusID", statusId);

            using var reader = cmd.ExecuteReader();
            var appointments = new List<AppointmentDTO>();

            while (reader.Read())
            {
                var appointment = new AppointmentDTO
                {
                    AppointmentID = reader.GetInt32(reader.GetOrdinal("appointmentID")),
                    MedicalHistoryID = reader.GetInt32(reader.GetOrdinal("medicalHistoryID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    ZoomLink = reader.IsDBNull(reader.GetOrdinal("zoomLink")) ? null : reader.GetString(reader.GetOrdinal("zoomLink")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                    Reason = reader.IsDBNull(reader.GetOrdinal("reason")) ? null : reader.GetString(reader.GetOrdinal("reason")),
                    IsAnonymous = reader.GetBoolean(reader.GetOrdinal("isAnonymous")),
                    AppointmentStatusID = reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    QueueNumber = reader.GetInt32(reader.GetOrdinal("queueNumber")),
                    AppointmentStatusName = reader.IsDBNull(reader.GetOrdinal("appointmentStatusName")) ? null : reader.GetString(reader.GetOrdinal("appointmentStatusName")),
                    DoctorName = reader.IsDBNull(reader.GetOrdinal("doctorName")) ? null : reader.GetString(reader.GetOrdinal("doctorName")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("startTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("endTime")),
                    Location = reader.IsDBNull(reader.GetOrdinal("location")) ? null : reader.GetString(reader.GetOrdinal("location")),
                    PatientName = reader.IsDBNull(reader.GetOrdinal("patientName")) ? null : reader.GetString(reader.GetOrdinal("patientName")),
                    PatientID = reader.GetInt32(reader.GetOrdinal("patientID")),
                    DoctorID = reader.GetInt32(reader.GetOrdinal("doctorID"))
                };

                appointments.Add(appointment);
            }

            return appointments;
        }

        public List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId, int statusId)
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
                a.queueNumber,
                s.appointmentStatusName,
                u.name AS doctorName,
                ds.date,
                ds.startTime,
                ds.endTime,
                ds.location,
                p.name AS patientName,
                m.patientID,
                m.doctorID
            FROM Appointment a
            INNER JOIN MedicalHistory m ON a.medicalHistoryID = m.medicalHistoryID
            INNER JOIN AppointmentStatus s ON a.appointmentStatusID = s.appointmentStatusID
            INNER JOIN DoctorSchedule ds ON a.doctorScheduleID = ds.doctorScheduleID
            INNER JOIN [User] u ON m.doctorID = u.userID
            INNER JOIN [User] p ON m.patientID = p.userID
            WHERE m.doctorID = @DoctorID AND a.appointmentStatusID = @StatusID
            ORDER BY ds.date, ds.startTime, a.queueNumber";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DoctorID", doctorId);
            cmd.Parameters.AddWithValue("@StatusID", statusId);

            using var reader = cmd.ExecuteReader();
            var appointments = new List<AppointmentDTO>();

            while (reader.Read())
            {
                var appointment = new AppointmentDTO
                {
                    AppointmentID = reader.GetInt32(reader.GetOrdinal("appointmentID")),
                    MedicalHistoryID = reader.GetInt32(reader.GetOrdinal("medicalHistoryID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    ZoomLink = reader.IsDBNull(reader.GetOrdinal("zoomLink")) ? null : reader.GetString(reader.GetOrdinal("zoomLink")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                    Reason = reader.IsDBNull(reader.GetOrdinal("reason")) ? null : reader.GetString(reader.GetOrdinal("reason")),
                    IsAnonymous = reader.GetBoolean(reader.GetOrdinal("isAnonymous")),
                    AppointmentStatusID = reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    QueueNumber = reader.GetInt32(reader.GetOrdinal("queueNumber")),
                    AppointmentStatusName = reader.IsDBNull(reader.GetOrdinal("appointmentStatusName")) ? null : reader.GetString(reader.GetOrdinal("appointmentStatusName")),
                    DoctorName = reader.IsDBNull(reader.GetOrdinal("doctorName")) ? null : reader.GetString(reader.GetOrdinal("doctorName")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("startTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("endTime")),
                    Location = reader.IsDBNull(reader.GetOrdinal("location")) ? null : reader.GetString(reader.GetOrdinal("location")),
                    PatientName = reader.IsDBNull(reader.GetOrdinal("patientName")) ? null : reader.GetString(reader.GetOrdinal("patientName")),
                    PatientID = reader.GetInt32(reader.GetOrdinal("patientID")),
                    DoctorID = reader.GetInt32(reader.GetOrdinal("doctorID"))
                };

                appointments.Add(appointment);
            }

            return appointments;
        }

        public void UpdateAppointmentStatus(int appointmentId, int newStatusId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            UPDATE Appointment
            SET appointmentStatusID = @NewStatusID
            WHERE appointmentID = @AppointmentID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NewStatusID", newStatusId);
            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new Exception($"No appointment found with ID {appointmentId}.");
            }
        }

        public int GetQueueCountByScheduleId(int scheduleId, int statusId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
        SELECT COUNT(*) 
        FROM Appointment 
        WHERE doctorScheduleID = @ScheduleID 
        AND AppointmentStatusID = @StatusID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ScheduleID", scheduleId);
            cmd.Parameters.AddWithValue("@StatusID", statusId);

            return (int)cmd.ExecuteScalar();
        }

        public int InsertMedicalHistory(MedicalHistoryDTO medicalHistory)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO MedicalHistory (dateTime, patientID, doctorID)
            OUTPUT INSERTED.medicalHistoryID
            VALUES (@DateTime, @PatientID, @DoctorID)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DateTime", medicalHistory.DateTime);
            cmd.Parameters.AddWithValue("@PatientID", medicalHistory.PatientID);
            cmd.Parameters.AddWithValue("@DoctorID", medicalHistory.DoctorID);

            return (int)cmd.ExecuteScalar();
        }

        public AppointmentDTO GetAppointmentById(int appointmentId)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            string query = @"
            SELECT 
                appointmentID,
                medicalHistoryID,
                doctorScheduleID,
                appointmentStatusID,
                zoomLink,
                notes,
                queueNumber
            FROM Appointment
            WHERE appointmentID = @AppointmentID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppointmentID", appointmentId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new AppointmentDTO
                {
                    AppointmentID = reader.GetInt32(reader.GetOrdinal("appointmentID")),
                    MedicalHistoryID = reader.GetInt32(reader.GetOrdinal("medicalHistoryID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    AppointmentStatusID = reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    ZoomLink = reader["zoomLink"]?.ToString(),
                    Notes = reader["notes"]?.ToString(),
                    QueueNumber = reader.GetInt32(reader.GetOrdinal("queueNumber"))
                };
            }

            return null;
        }

        public List<AppointmentDTO> GetConfirmedAppointmentsByScheduleId(int scheduleId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT appointmentID, queueNumber
            FROM Appointment
            WHERE doctorScheduleID = @ScheduleID
              AND appointmentStatusID = @StatusID
            ORDER BY queueNumber ASC";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ScheduleID", scheduleId);
            cmd.Parameters.AddWithValue("@StatusID", AppointmentStatusEnum.Confirmed);

            using var reader = cmd.ExecuteReader();
            var list = new List<AppointmentDTO>();

            while (reader.Read())
            {
                list.Add(new AppointmentDTO
                {
                    AppointmentID = reader.GetInt32(reader.GetOrdinal("appointmentID")),
                    QueueNumber = reader.GetInt32(reader.GetOrdinal("queueNumber"))
                });
            }

            return list;
        }

        public void UpdateQueueNumber(int appointmentId, int? newQueueNumber)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            UPDATE Appointment
            SET queueNumber = @NewQueue
            WHERE appointmentID = @AppointmentID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NewQueue", newQueueNumber);
            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);

            cmd.ExecuteNonQuery();
        }

        public AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            string query = @"
            SELECT 
                a.appointmentID,
                a.medicalHistoryID,
                a.doctorScheduleID,
                a.appointmentStatusID,
                a.zoomLink,
                a.notes,
                a.reason,
                a.queueNumber,

                u.userID AS patientUserID,
                u.name AS patientName,
                u.phone AS patientPhone,
                u.dob AS patientDob,

                pp.height,
                pp.weight,
                pp.gender,

                hs.HIVStatusName,
                ps.pregnancyStatusName

            FROM Appointment a
            INNER JOIN MedicalHistory m ON a.medicalHistoryID = m.medicalHistoryID
            INNER JOIN [User] u ON m.patientID = u.userID
            LEFT JOIN PatientProfile pp ON pp.userID = u.userID
            LEFT JOIN HIVStatus hs ON pp.hivStatusID = hs.HIVStatusID
            LEFT JOIN PregnancyStatus ps ON pp.pregnancyStatusID = ps.pregnancyStatusID
            WHERE a.appointmentID = @AppointmentID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppointmentID", appointmentId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new AppointmentDTO
                {
                    AppointmentID = reader.GetInt32(reader.GetOrdinal("appointmentID")),
                    MedicalHistoryID = reader.GetInt32(reader.GetOrdinal("medicalHistoryID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    AppointmentStatusID = reader.IsDBNull(reader.GetOrdinal("appointmentStatusID")) ? null : reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    ZoomLink = reader["zoomLink"]?.ToString(),
                    Notes = reader["notes"]?.ToString(),
                    Reason = reader["reason"]?.ToString(),
                    QueueNumber = reader.IsDBNull(reader.GetOrdinal("queueNumber")) ? null : reader.GetInt32(reader.GetOrdinal("queueNumber")),

                    PatientID = reader.GetInt32(reader.GetOrdinal("patientUserID")),
                    PatientName = reader["patientName"]?.ToString(),
                    PatientDob = reader.GetDateTime(reader.GetOrdinal("patientDob")),
                    PatientPhone = reader["patientPhone"]?.ToString(),

                    PatientProfile = new PatientProfileDTO
                    {
                        Height = reader.IsDBNull(reader.GetOrdinal("height")) ? null : reader.GetFloat(reader.GetOrdinal("height")),
                        Weight = reader.IsDBNull(reader.GetOrdinal("weight")) ? null : reader.GetFloat(reader.GetOrdinal("weight")),
                        Gender = reader["gender"]?.ToString(),
                        HIVStatusName = reader["HIVStatusName"]?.ToString(),
                        PregnancyStatusName = reader["pregnancyStatusName"]?.ToString(),
                    }
                };
            }

            return null;
        }
    }
}
