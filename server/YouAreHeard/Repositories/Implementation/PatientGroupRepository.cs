using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class PatientGroupRepository : IPatientGroupRepository
    {
        public List<PatientGroupDTO> GetAllPatientGroups()
        {
            var groups = new List<PatientGroupDTO>();

            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT patientGroupID, patientGroupName FROM PatientGroup";
            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                groups.Add(new PatientGroupDTO
                {
                    PatientGroupID = reader.GetInt32(0),
                    PatientGroupName = reader.GetString(1)
                });
            }

            return groups;
        }
    }
}
