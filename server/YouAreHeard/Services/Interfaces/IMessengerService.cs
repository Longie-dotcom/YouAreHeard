using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IMessengerService
    {
        Task SendReminderAsync(PillRemindTimesDTO reminder);
        Task SendTextMessageAsync(string psid, string text);
        Task SendReminderButtonAsync(string psid);
    }
}
