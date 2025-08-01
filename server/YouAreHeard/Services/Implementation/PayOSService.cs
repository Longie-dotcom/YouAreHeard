using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using YouAreHeard.Enums;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;
using YouAreHeard.Utilities;

namespace YouAreHeard.Services.Implementation
{
    public class PayOSService : IPayOSService
    {
        private readonly string _clientId = PayOSSettingContext.Settings.ClientId;
        private readonly string _apiKey = PayOSSettingContext.Settings.ApiKey;
        private readonly string _checksumKey = PayOSSettingContext.Settings.ChecksumKey;
        private readonly string _returnUrl = PayOSSettingContext.Settings.ReturnUrl;
        private readonly string _cancelUrl = PayOSSettingContext.Settings.CancelUrl;
        private readonly string _endpoint = "https://api-merchant.payos.vn/v2/payment-requests";

        public string GeneratePaymentUrl(PayOSPaymentRequest request)
        {
            int orderCode = request.OrderCode;
            int amount = request.Amount;
            string description = request.Description.Length > 9 ? request.Description.Substring(0, 9) : request.Description;

            long expiredAt = DateTimeOffset.UtcNow
                .AddMinutes(Constraints.ExpiredPendingAppointment)
                .ToUnixTimeSeconds(); // NEW
            string signature = GeneratePayOSSignature(
                orderCode,
                amount,
                description,
                _returnUrl,
                _cancelUrl,
                _checksumKey
            );

            var payload = new
            {
                orderCode = orderCode,
                amount = amount,
                description = description,
                buyerName = request.BuyerName ?? "Anonymous",
                buyerEmail = request.BuyerEmail ?? "test@example.com",
                buyerPhone = request.BuyerPhone ?? "0123456789",
                buyerAddress = request.BuyerAddress ?? "Vietnam",
                items = request.Items ?? new List<object>(),
                cancelUrl = _cancelUrl,
                returnUrl = _returnUrl,
                expiredAt = expiredAt,
                signature = signature
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            Console.WriteLine("📤 Sending to PayOS:");
            Console.WriteLine(jsonPayload);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-client-id", _clientId);
            client.DefaultRequestHeaders.Add("x-api-key", _apiKey);

            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = client.PostAsync(_endpoint, httpContent).Result;
            string responseJson = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("📥 PayOS Response:");
            Console.WriteLine(responseJson);

            var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
            var checkoutUrl = result?.data?.checkoutUrl?.ToString();
            return $"{checkoutUrl}?expiredAt={expiredAt}";
        }

        public string GeneratePayOSSignature(int orderCode, int amount, string description, string returnUrl, string cancelUrl, string checksumKey)
        {
            string rawData = $"amount={amount}&cancelUrl={cancelUrl}&description={description}&orderCode={orderCode}&returnUrl={returnUrl}";

            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(checksumKey);
            byte[] messageBytes = encoding.GetBytes(rawData);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}