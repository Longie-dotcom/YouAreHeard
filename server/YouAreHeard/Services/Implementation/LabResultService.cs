using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class LabResultService : ILabResultService
    {
        private readonly ILabResultRepository _LabResultRepository;
        private readonly ITestStageRepository _TestStageRepository;
        private readonly ITestTypeRepository _TestTypeRepository;

        public LabResultService(ILabResultRepository labResultRepository, ITestStageRepository testStageRepository, ITestTypeRepository testTypeRepository)
        {
            _LabResultRepository = labResultRepository;
            _TestStageRepository = testStageRepository;
            _TestTypeRepository = testTypeRepository;
        }

        public List<LabResultDTO> GetAllLabResults()
        {
            return _LabResultRepository.GetAllLabResults();
        }

        public List<TestStageDTO> GetAllTestStages()
        {
            return _TestStageRepository.GetAllTestStages();
        }

        public List<TestTypeDTO> GetAllTestTypes()
        {
            return _TestTypeRepository.GetAllTestTypes();
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

        public int InsertLabResult(LabResultDTO lr)
        {
            return _LabResultRepository.InsertLabResult(lr);
        }
    }
}