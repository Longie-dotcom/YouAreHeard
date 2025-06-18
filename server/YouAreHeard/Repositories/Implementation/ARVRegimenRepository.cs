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
                r.regimenSideEffects,
                r.regimenIndications,
                r.regimenContraindications,
                mc.dosage AS medDosage,
                mc.frequency AS medFrequency,
                m.medicationID,
                m.medicationName,
                m.dosageMetric,
                m.sideEffect,
                m.contraindications,
                m.indications
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
                        RegimenSideEffects = reader.IsDBNull(reader.GetOrdinal("regimenSideEffects"))
                        ? "" : reader.GetString(reader.GetOrdinal("regimenSideEffects")),
                                            RegimenIndications = reader.IsDBNull(reader.GetOrdinal("regimenIndications"))
                        ? "" : reader.GetString(reader.GetOrdinal("regimenIndications")),
                                            RegimenContraindications = reader.IsDBNull(reader.GetOrdinal("regimenContraindications"))
                        ? "" : reader.GetString(reader.GetOrdinal("regimenContraindications")),
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
                        MedicationName = reader.GetString(reader.GetOrdinal("medicationName")),
                        DosageMetric = reader.IsDBNull(reader.GetOrdinal("dosageMetric")) ? "" : reader.GetString(reader.GetOrdinal("dosageMetric")),
                        SideEffect = reader.IsDBNull(reader.GetOrdinal("sideEffect")) ? "" : reader.GetString(reader.GetOrdinal("sideEffect")),
                        Contraindications = reader.IsDBNull(reader.GetOrdinal("contraindications")) ? "" : reader.GetString(reader.GetOrdinal("contraindications")),
                        Indications = reader.IsDBNull(reader.GetOrdinal("indications")) ? "" : reader.GetString(reader.GetOrdinal("indications")),
                        Dosage = reader.IsDBNull(reader.GetOrdinal("medDosage")) ? 0 : reader.GetInt32(reader.GetOrdinal("medDosage")),
                        Frequency = reader.IsDBNull(reader.GetOrdinal("medFrequency")) ? 0 : reader.GetInt32(reader.GetOrdinal("medFrequency"))
                    };

                    current?.Medications.Add(medication);
                }
            }

            return regimens;
        }

    }
}
