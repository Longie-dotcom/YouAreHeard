using YouAreHeard.Models;

namespace YouAreHeard.Services
{
    public interface ILabTestService
    {
        List<TestStageDTO> GetAllTestStages();

        TestStageDTO GetTestStageById(int id);

        List<TestTypeDTO> GetAllTestTypesWithMetrics();

        TestTypeDTO GetTestTypeById(int id);

        List<LabResultDTO> GetAllLabResults();

        List<LabResultDTO> GetLabResultByPatientId(int id);

        List<LabResultDTO> GetLabResultByDoctorId(int id);

        void CreateLabResult(LabResultDTO lr);

        List<TestMetricDTO> GetTestMetrics();
    } 
}