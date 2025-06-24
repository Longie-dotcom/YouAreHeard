using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly ILabResultRepository _LabResultRepository;
        private readonly ITestStageRepository _TestStageRepository;
        private readonly ITestTypeRepository _TestTypeRepository;
        private readonly ITestMetricRepository _TestMetricRepository;
        private readonly ITestMetricValueRepository _TestMetricValueRepository;

        public LabTestService(
            ILabResultRepository labResultRepository, 
            ITestStageRepository testStageRepository, 
            ITestTypeRepository testTypeRepository,
            ITestMetricRepository testMetricRepository,
            ITestMetricValueRepository testMetricValueRepository)
        {
            _LabResultRepository = labResultRepository;
            _TestStageRepository = testStageRepository;
            _TestTypeRepository = testTypeRepository;
            _TestMetricRepository = testMetricRepository;
            _TestMetricValueRepository = testMetricValueRepository;
        }

        public List<LabResultDTO> GetAllLabResults()
        {
            return _LabResultRepository.GetAllLabResults();
        }

        public List<TestStageDTO> GetAllTestStages()
        {
            return _TestStageRepository.GetAllTestStages();
        }

        public List<TestTypeDTO> GetAllTestTypesWithMetrics()
        {
            return _TestTypeRepository.GetAllTestTypesWithMetrics();
        }

        public List<LabResultDTO> GetLabResultByDoctorId(int id)
        {
            return _LabResultRepository.GetLabResultByDoctorId(id);
        }

        public List<LabResultDTO> GetLabResultByPatientId(int id)
        {
            return _LabResultRepository.GetLabResultByPatientId(id);
        }

        public TestStageDTO GetTestStageById(int id)
        {
            return _TestStageRepository.GetTestStageById(id);
        }

        public TestTypeDTO GetTestTypeById(int id)
        {
            return _TestTypeRepository.GetTestTypeById(id);
        }

        public void CreateLabResult(LabResultDTO lr)
        {
            var labResultId = _LabResultRepository.InsertLabResult(lr);

            foreach (var metric in lr.testMetricValues)
            {
                var testMetricValue = new TestMetricValueDTO
                {
                    LabResultID = labResultId,
                    TestMetricID = metric.TestMetricID,
                    Value = metric.Value
                };

                _TestMetricValueRepository.InsertTestMetricValue(testMetricValue);
            }
        }

        public List<TestMetricDTO> GetTestMetrics()
        {
            return _TestMetricRepository.GetAllTestMetrics();
        }
    }
}