using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPillRemindTimesRepository
    {
        public void insertPillRemindTimes(PillRemindTimesDTO pillRemindTimes);
        List<(string FacebookId, string MedicationName, string DosageMetric, int Dosage, TimeSpan Time)> GetPillRemindersDueNow();
    }
}