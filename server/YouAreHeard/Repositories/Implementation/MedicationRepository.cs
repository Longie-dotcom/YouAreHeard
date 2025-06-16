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

            string query = "SELECT medicationID, medicationName FROM Medication";
            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                medications.Add(new MedicationDTO
                {
                    MedicationID = reader.GetInt32(0),
                    MedicationName = reader.GetString(1)
                });
            }

            return medications;
        }
    }
}
