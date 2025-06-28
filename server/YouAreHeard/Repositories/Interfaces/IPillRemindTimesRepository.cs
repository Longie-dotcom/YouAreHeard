using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPillRemindTimesRepository
    {
        public void insertPillRemindTimes(PillRemindTimesDTO pillRemindTimes);
    }
}