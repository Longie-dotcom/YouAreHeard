using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class ARVRegimenRepository : IARVRegimenRepository
    {
        public List<ARVRegimenDTO> GetAllARVRegimens()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    r.regimenID,
                    r.name,
                    r.type,
                    r.duration,
                    r.sideEffects,
                    r.dosage,
                    r.frequency,
                    r.indications,
                    r.contraindications,
                    m.medicationID,
                    m.medicationName
                FROM ARVRegimen r
                LEFT JOIN MedicationCombination mc ON r.regimenID = mc.regimenID
                LEFT JOIN Medication m ON mc.medicationID = m.medicationID
                ORDER BY r.regimenID";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var regimens = new List<ARVRegimenDTO>();
            ARVRegimenDTO? current = null;
            int lastRegimenId = -1;

            while (reader.Read())
            {
                int regimenID = reader.GetInt32(reader.GetOrdinal("regimenID"));

                if (regimenID != lastRegimenId)
                {
                    current = new ARVRegimenDTO
                    {
                        RegimenID = regimenID,
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Type = reader.GetString(reader.GetOrdinal("type")),
                        Duration = reader.IsDBNull(reader.GetOrdinal("duration")) ? "" : reader.GetString(reader.GetOrdinal("duration")),
                        SideEffects = reader.IsDBNull(reader.GetOrdinal("sideEffects")) ? "" : reader.GetString(reader.GetOrdinal("sideEffects")),
                        Dosage = reader.IsDBNull(reader.GetOrdinal("dosage")) ? "" : reader.GetString(reader.GetOrdinal("dosage")),
                        Frequency = reader.IsDBNull(reader.GetOrdinal("frequency")) ? "" : reader.GetString(reader.GetOrdinal("frequency")),
                        Indications = reader.IsDBNull(reader.GetOrdinal("indications")) ? "" : reader.GetString(reader.GetOrdinal("indications")),
                        Contraindications = reader.IsDBNull(reader.GetOrdinal("contraindications")) ? "" : reader.GetString(reader.GetOrdinal("contraindications")),
                        Medications = new List<MedicationDTO>()
                    };

                    regimens.Add(current);
                    lastRegimenId = regimenID;
                }

                if (!reader.IsDBNull(reader.GetOrdinal("medicationID")))
                {
                    var medication = new MedicationDTO
                    {
                        MedicationID = reader.GetInt32(reader.GetOrdinal("medicationID")),
                        MedicationName = reader.GetString(reader.GetOrdinal("medicationName"))
                    };

                    current?.Medications.Add(medication);
                }
            }

            return regimens;
        }
    }
}
