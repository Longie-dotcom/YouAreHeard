using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ILabResultRepository
    {
        public List<LabResultDTO> GetAllLabResults();

        public List<LabResultDTO> GetLabResultByPatientId(int id);

        public List<LabResultDTO> GetLabResultByDoctorId(int id);

        public int InsertLabResult(LabResultDTO lr);
    }
}