using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.BackgroundServices
{
    public class PillReminderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PillReminderBackgroundService> _logger;

        public PillReminderBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<PillReminderBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var reminderService = scope.ServiceProvider.GetRequiredService<IPillRemindTimesRepository>();
                    var messengerService = scope.ServiceProvider.GetRequiredService<IMessengerService>();

                    var reminders = reminderService.GetPillRemindersDueNow();

                    foreach (var reminder in reminders)
                    {
                        var msg = $"Sending reminder to {reminder.FacebookId} for {reminder.Dosage} {reminder.DosageMetric} {reminder} {reminder.MedicationName} at {reminder.Time}";
                        _logger.LogInformation(msg);

                        await messengerService.SendReminderAsync(new PillRemindTimesDTO
                        {
                            FacebookId = reminder.FacebookId,
                            MedicationName = reminder.MedicationName,
                            Time = reminder.Time.ToString(),
                            DosageMetric = reminder.DosageMetric,
                            DrinkDosage = reminder.Dosage
                        });
                    }
                    _logger.LogInformation("PillReminderBackgroundService ran at: {Time}", DateTime.Now);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
