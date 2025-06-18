using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace YouAreHeard.Repositories.Implementation
{
    public class MedicationRepository : IMedicationRepository
    {
        public List<MedicationDTO> GetAllMedications()
        {
            var medications = new List<MedicationDTO>();
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT 
                medicationID, 
                medicationName, 
                dosageMetric, 
                sideEffect, 
                contraindications, 
                indications
            FROM Medication";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                medications.Add(new MedicationDTO
                {
                    MedicationID = reader.GetInt32(0),
                    MedicationName = reader.GetString(1),
                    DosageMetric = reader.IsDBNull(2) ? null : reader.GetString(2),
                    SideEffect = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Contraindications = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Indications = reader.IsDBNull(5) ? null : reader.GetString(5),

                    Dosage = 0,
                    Frequency = 0
                });
            }

            return medications;
        }

    }
}
