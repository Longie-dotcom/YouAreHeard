using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;
using YouAreHeard.Utilities;

namespace YouAreHeard.Services
{
    public class MessengerService : IMessengerService
    {
        private readonly HttpClient _httpClient;
        private const string PAGE_ACCESS_TOKEN =
            "EAAKo9uhkCIMBPHb7vi7vgJR6T0zIyr8pVH03CpSQ0UUWBZCgt6nM6XNyNZA0Yu0zZCDediJZBzrZBZAIgNNvzBptASYgvIshQZCGjtZBhLdaoUZBA57qZAzvaWciGCtig7xO4ZBbosaZABKt9jGlUTCNm0xyu1GmguVvl9CTD2MB0QUJeC77mAjyxZAU966f0MEZCWB42Qp7MvWgZDZD";

        public MessengerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendReminderAsync(PillRemindTimesDTO reminder)
        {
            var payload = new
            {
                messaging_type = "MESSAGE_TAG",
                tag = "ACCOUNT_UPDATE",
                recipient = new { id = reminder.FacebookId },
                message = new
                {
                    text = $"💊 Nhắc bạn uống {reminder.DrinkDosage} {reminder.DosageMetric} {reminder.MedicationName} lúc {reminder.Time}"
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://graph.facebook.com/v18.0/me/messages?access_token={PAGE_ACCESS_TOKEN}";
            var response = await _httpClient.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[MessengerService] Facebook API error: {responseText}");
            }
        }

        public async Task SendTextMessageAsync(string psid, string text)
        {
            var url = $"https://graph.facebook.com/v18.0/me/messages?access_token={PAGE_ACCESS_TOKEN}";

            var payload = new
            {
                messaging_type = "RESPONSE",
                recipient = new { id = psid },
                message = new { text = text }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendReminderButtonAsync(string psid)
        {
            var url = $"https://graph.facebook.com/v18.0/me/messages?access_token={PAGE_ACCESS_TOKEN}";

            var payload = new
            {
                recipient = new { id = psid },
                message = new
                {
                    attachment = new
                    {
                        type = "template",
                        payload = new
                        {
                            template_type = "button",
                            text = "Bạn có muốn được nhắc nhở uống thuốc không?",
                            buttons = new[]
                            {
                        new {
                            type = "postback",
                            title = "Tôi muốn được nhắc nhở uống thuốc",
                            payload = "REGISTER_REMINDER"
                        }
                    }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
    }
}