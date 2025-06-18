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
            (doctorID, patientID, doctorScheduleID, appointmentStatusID, zoomLink, notes, reason, isAnonymous, isOnline, queueNumber, date)
            OUTPUT INSERTED.appointmentID
            VALUES 
            (@DoctorID, @PatientID, @DoctorScheduleID, @AppointmentStatusID, @ZoomLink, @Notes, @Reason, @IsAnonymous, @IsOnline, @QueueNumber, @CreatedDate)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DoctorID", appointment.DoctorID);
            cmd.Parameters.AddWithValue("@PatientID", appointment.PatientID);
            cmd.Parameters.AddWithValue("@DoctorScheduleID", appointment.DoctorScheduleID);
            cmd.Parameters.AddWithValue("@AppointmentStatusID", (object?)appointment.AppointmentStatusID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ZoomLink", (object?)appointment.ZoomLink ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", (object?)appointment.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Reason", (object?)appointment.Reason ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsAnonymous", appointment.IsAnonymous);
            cmd.Parameters.AddWithValue("@IsOnline", appointment.IsOnline);
            cmd.Parameters.AddWithValue("@QueueNumber", (object?)appointment.QueueNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", appointment.CreatedDate);

            return (int)cmd.ExecuteScalar();
        }

        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId, int statusId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT 
                a.appointmentID,
                a.doctorID,
                a.patientID,
                a.doctorScheduleID,
                a.zoomLink,
                a.notes,
                a.reason,
                a.isAnonymous,
                a.isOnline,
                a.appointmentStatusID,
                a.queueNumber,
                a.date AS createdDate,
                s.appointmentStatusName,
                doc.name AS doctorName,
                pat.name AS patientName,
                pat.phone AS patientPhone,
                pat.dob AS patientDob,
                ds.date,
                ds.startTime,
                ds.endTime,
                ds.location
            FROM Appointment a
            INNER JOIN AppointmentStatus s ON a.appointmentStatusID = s.appointmentStatusID
            INNER JOIN DoctorSchedule ds ON a.doctorScheduleID = ds.doctorScheduleID
            INNER JOIN [User] doc ON a.doctorID = doc.userID
            INNER JOIN [User] pat ON a.patientID = pat.userID
            WHERE a.patientID = @PatientID AND a.appointmentStatusID = @StatusID
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
                    DoctorID = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    PatientID = reader.GetInt32(reader.GetOrdinal("patientID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    ZoomLink = reader.IsDBNull(reader.GetOrdinal("zoomLink")) ? null : reader.GetString(reader.GetOrdinal("zoomLink")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                    Reason = reader.IsDBNull(reader.GetOrdinal("reason")) ? null : reader.GetString(reader.GetOrdinal("reason")),
                    IsAnonymous = reader.GetBoolean(reader.GetOrdinal("isAnonymous")),
                    IsOnline = reader.GetBoolean(reader.GetOrdinal("isOnline")),
                    AppointmentStatusID = reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    QueueNumber = reader.IsDBNull(reader.GetOrdinal("queueNumber")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("queueNumber")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("createdDate")),

                    AppointmentStatusName = reader.IsDBNull(reader.GetOrdinal("appointmentStatusName")) ? null : reader.GetString(reader.GetOrdinal("appointmentStatusName")),
                    DoctorName = reader.IsDBNull(reader.GetOrdinal("doctorName")) ? null : reader.GetString(reader.GetOrdinal("doctorName")),
                    PatientName = reader.IsDBNull(reader.GetOrdinal("patientName")) ? null : reader.GetString(reader.GetOrdinal("patientName")),
                    PatientPhone = reader.IsDBNull(reader.GetOrdinal("patientPhone")) ? null : reader.GetString(reader.GetOrdinal("patientPhone")),
                    PatientDob = reader.IsDBNull(reader.GetOrdinal("patientDob")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("patientDob")),

                    ScheduleDate = reader.GetDateTime(reader.GetOrdinal("date")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("startTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("endTime")),
                    Location = reader.GetString(reader.GetOrdinal("location"))
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
                a.doctorID,
                a.patientID,
                a.doctorScheduleID,
                a.zoomLink,
                a.notes,
                a.reason,
                a.isAnonymous,
                a.isOnline,
                a.appointmentStatusID,
                a.queueNumber,
                a.date AS createdDate,
                s.appointmentStatusName,
                doc.name AS doctorName,
                pat.name AS patientName,
                pat.phone AS patientPhone,
                pat.dob AS patientDob,
                ds.date,
                ds.startTime,
                ds.endTime,
                ds.location
            FROM Appointment a
            INNER JOIN AppointmentStatus s ON a.appointmentStatusID = s.appointmentStatusID
            INNER JOIN DoctorSchedule ds ON a.doctorScheduleID = ds.doctorScheduleID
            INNER JOIN [User] doc ON a.doctorID = doc.userID
            INNER JOIN [User] pat ON a.patientID = pat.userID
            WHERE a.doctorID = @DoctorID AND a.appointmentStatusID = @StatusID
            ORDER BY ds.date ASC, ds.startTime ASC, a.queueNumber ASC";

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
                    DoctorID = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    PatientID = reader.GetInt32(reader.GetOrdinal("patientID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    ZoomLink = reader.IsDBNull(reader.GetOrdinal("zoomLink")) ? null : reader.GetString(reader.GetOrdinal("zoomLink")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                    Reason = reader.IsDBNull(reader.GetOrdinal("reason")) ? null : reader.GetString(reader.GetOrdinal("reason")),
                    IsAnonymous = reader.GetBoolean(reader.GetOrdinal("isAnonymous")),
                    IsOnline = reader.GetBoolean(reader.GetOrdinal("isOnline")),
                    AppointmentStatusID = reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    QueueNumber = reader.IsDBNull(reader.GetOrdinal("queueNumber")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("queueNumber")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("createdDate")),

                    AppointmentStatusName = reader.IsDBNull(reader.GetOrdinal("appointmentStatusName")) ? null : reader.GetString(reader.GetOrdinal("appointmentStatusName")),
                    DoctorName = reader.IsDBNull(reader.GetOrdinal("doctorName")) ? null : reader.GetString(reader.GetOrdinal("doctorName")),
                    PatientName = reader.IsDBNull(reader.GetOrdinal("patientName")) ? null : reader.GetString(reader.GetOrdinal("patientName")),
                    PatientPhone = reader.IsDBNull(reader.GetOrdinal("patientPhone")) ? null : reader.GetString(reader.GetOrdinal("patientPhone")),
                    PatientDob = reader.IsDBNull(reader.GetOrdinal("patientDob")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("patientDob")),

                    ScheduleDate = reader.GetDateTime(reader.GetOrdinal("date")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("startTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("endTime")),
                    Location = reader.GetString(reader.GetOrdinal("location"))
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

        public AppointmentDTO GetAppointmentById(int appointmentId)
        {
            using var connection = DBContext.GetConnection();
            connection.Open();

            string query = @"
            SELECT 
                appointmentID,
                doctorScheduleID,
                appointmentStatusID,
                zoomLink,
                notes,
                reason,
                isAnonymous,
                isOnline,
                queueNumber,
                date,
                doctorID,
                patientID
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
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    AppointmentStatusID = reader.IsDBNull(reader.GetOrdinal("appointmentStatusID"))
                        ? (int?)null : reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    ZoomLink = reader.IsDBNull(reader.GetOrdinal("zoomLink"))
                        ? null : reader.GetString(reader.GetOrdinal("zoomLink")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("notes"))
                        ? null : reader.GetString(reader.GetOrdinal("notes")),
                    Reason = reader.IsDBNull(reader.GetOrdinal("reason"))
                        ? null : reader.GetString(reader.GetOrdinal("reason")),
                    IsAnonymous = reader.GetBoolean(reader.GetOrdinal("isAnonymous")),
                    IsOnline = reader.GetBoolean(reader.GetOrdinal("isOnline")),
                    QueueNumber = reader.IsDBNull(reader.GetOrdinal("queueNumber"))
                        ? (int?)null : reader.GetInt32(reader.GetOrdinal("queueNumber")),
                    CreatedDate = reader.IsDBNull(reader.GetOrdinal("date"))
                        ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("date")),
                    DoctorID = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    PatientID = reader.GetInt32(reader.GetOrdinal("patientID"))
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
            INNER JOIN [User] u ON a.patientID = u.userID
            LEFT JOIN PatientProfile pp ON pp.userID = u.userID
            LEFT JOIN HIVStatus hs ON pp.hivStatusID = hs.HIVStatusID
            LEFT JOIN PregnancyStatus ps ON pp.pregnancyStatusID = ps.PregnancyStatusID
            WHERE a.appointmentID = @AppointmentID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppointmentID", appointmentId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new AppointmentDTO
                {
                    AppointmentID = reader.GetInt32(reader.GetOrdinal("appointmentID")),
                    DoctorScheduleID = reader.GetInt32(reader.GetOrdinal("doctorScheduleID")),
                    AppointmentStatusID = reader.IsDBNull(reader.GetOrdinal("appointmentStatusID"))
                        ? null : reader.GetInt32(reader.GetOrdinal("appointmentStatusID")),
                    ZoomLink = reader.IsDBNull(reader.GetOrdinal("zoomLink"))
                        ? null : reader.GetString(reader.GetOrdinal("zoomLink")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("notes"))
                        ? null : reader.GetString(reader.GetOrdinal("notes")),
                    Reason = reader.IsDBNull(reader.GetOrdinal("reason"))
                        ? null : reader.GetString(reader.GetOrdinal("reason")),
                    QueueNumber = reader.IsDBNull(reader.GetOrdinal("queueNumber"))
                        ? null : reader.GetInt32(reader.GetOrdinal("queueNumber")),

                    PatientID = reader.GetInt32(reader.GetOrdinal("patientUserID")),
                    PatientName = reader.IsDBNull(reader.GetOrdinal("patientName"))
                        ? null : reader.GetString(reader.GetOrdinal("patientName")),
                    PatientPhone = reader.IsDBNull(reader.GetOrdinal("patientPhone"))
                        ? null : reader.GetString(reader.GetOrdinal("patientPhone")),
                    PatientDob = reader.IsDBNull(reader.GetOrdinal("patientDob"))
                        ? null : reader.GetDateTime(reader.GetOrdinal("patientDob")),

                    PatientProfile = new PatientProfileDTO
                    {
                        Height = reader.IsDBNull(reader.GetOrdinal("height"))
                        ? null : (float?)reader.GetDouble(reader.GetOrdinal("height")),
                        Weight = reader.IsDBNull(reader.GetOrdinal("weight"))
                        ? null : (float?)reader.GetDouble(reader.GetOrdinal("weight")),
                        Gender = reader.IsDBNull(reader.GetOrdinal("gender"))
                        ? null : reader.GetString(reader.GetOrdinal("gender")),
                        HIVStatusName = reader.IsDBNull(reader.GetOrdinal("HIVStatusName"))
                        ? null : reader.GetString(reader.GetOrdinal("HIVStatusName")),
                        PregnancyStatusName = reader.IsDBNull(reader.GetOrdinal("pregnancyStatusName"))
                        ? null : reader.GetString(reader.GetOrdinal("pregnancyStatusName"))
                    }
                };
            }

            return null;
        }
    }
}
