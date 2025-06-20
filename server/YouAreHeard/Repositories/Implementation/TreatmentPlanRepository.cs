using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class TreatmentPlanRepository : ITreatmentPlanRepository
    {
        public int insertTreatmentPlan(TreatmentPlanDTO treatmentPlan)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO TreatmentPlan
            (regimenID, doctorID, patientID, date, patientGroupID, notes)
            OUTPUT INSERTED.treatmentPlanID
            VALUES
            (@RegimenID, @DoctorID, @PatientID, @Date, @PatientGroupID, @Notes)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@RegimenID", treatmentPlan.RegimenID);
            cmd.Parameters.AddWithValue("@DoctorID", treatmentPlan.DoctorID);
            cmd.Parameters.AddWithValue("@PatientID", treatmentPlan.PatientID);
            cmd.Parameters.AddWithValue("@Date", treatmentPlan.Date);
            cmd.Parameters.AddWithValue("@PatientGroupID", treatmentPlan.PatientGroupID);
            cmd.Parameters.AddWithValue("@Notes", (object?)treatmentPlan.Notes ?? DBNull.Value);

            return (int)cmd.ExecuteScalar();
        }

        public TreatmentPlanDetailsDTO? GetLatestTreatmentPlanByPatientID(int patientID)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT 
                tp.treatmentPlanID, tp.regimenID, tp.doctorID, tp.patientID, tp.date, tp.patientGroupID, tp.notes,

                ar.name AS regimenName, ar.type, ar.duration,
                ar.regimenSideEffects, ar.regimenIndications, ar.regimenContraindications,

                prt.time, prt.drinkDosage, prt.medicationID,

                m.medicationName, m.dosageMetric

            FROM TreatmentPlan tp
            JOIN ARVRegimen ar ON tp.regimenID = ar.regimenID
            LEFT JOIN PillRemindTimes prt ON tp.treatmentPlanID = prt.treatmentPlanID
            LEFT JOIN Medication m ON prt.medicationID = m.medicationID
            WHERE tp.patientID = @PatientID AND tp.date = (
                SELECT MAX(date) FROM TreatmentPlan WHERE patientID = @PatientID
            )
            ORDER BY prt.time";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", patientID);

            using var reader = cmd.ExecuteReader();

            TreatmentPlanDetailsDTO? treatmentPlan = null;

            while (reader.Read())
            {
                if (treatmentPlan == null)
                {
                    treatmentPlan = new TreatmentPlanDetailsDTO
                    {
                        TreatmentPlanID = reader.GetInt32(0),
                        RegimenID = reader.GetInt32(1),
                        DoctorID = reader.GetInt32(2),
                        PatientID = reader.GetInt32(3),
                        Date = reader.GetDateTime(4),
                        PatientGroupID = reader.GetInt32(5),
                        Notes = reader.IsDBNull(6) ? null : reader.GetString(6),

                        RegimenName = reader.GetString(7),
                        RegimenType = reader.GetString(8),
                        RegimenDuration = reader.GetString(9),
                        RegimenSideEffects = reader.GetString(10),
                        RegimenIndications = reader.GetString(11),
                        RegimenContraindications = reader.GetString(12),

                        PillRemindTimes = new List<PillRemindTimesDTO>()
                    };
                }

                // Only add pill if there's a valid time
                if (!reader.IsDBNull(13))
                {
                    treatmentPlan.PillRemindTimes.Add(new PillRemindTimesDTO
                    {
                        Time = reader.GetTimeSpan(13).ToString(@"hh\:mm"),
                        DrinkDosage = reader.GetInt32(14),
                        MedicationID = reader.GetInt32(15),
                        MedicationName = reader.IsDBNull(16) ? null : reader.GetString(16),
                        DosageMetric = reader.IsDBNull(17) ? null : reader.GetString(17),
                        TreatmentPlanID = treatmentPlan.TreatmentPlanID
                    });
                }
            }

            return treatmentPlan;
        }
    }
}
