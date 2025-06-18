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
    }
}
