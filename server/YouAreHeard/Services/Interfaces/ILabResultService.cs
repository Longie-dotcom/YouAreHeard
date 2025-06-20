using YouAreHeard.Models;

namespace YouAreHeard.Services
{
    public interface ILabResultService
    {
        List<TestStageDTO> GetAllTestStages();

        TestStageDTO GetTestStageById(int id);

        List<TestTypeDTO> GetAllTestTypes();

        TestTypeDTO GetTestTypeById(int id);

        List<LabResultDTO> GetAllLabResults();

        List<LabResultDTO> GetLabResultByPatientId(int id);

        List<LabResultDTO> GetLabResultByDoctorId(int id);

        int InsertLabResult(LabResultDTO lr);
    } 
}