using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YouAreHeard.Services.Implementation;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessengerController : ControllerBase
    {
        private readonly ILogger<MessengerController> _logger;
        private readonly IConfiguration _config;
        private readonly string _verifyToken;
        private readonly IMessengerService _messengerService;
        private readonly IUserService _userService;

        public MessengerController(
            ILogger<MessengerController> logger, 
            IConfiguration config, 
            IMessengerService messengerService,
            IUserService userService)
        {
            _userService = userService;
            _messengerService = messengerService;
            _logger = logger;
            _config = config;
            _verifyToken = _config["Facebook:VerifyToken"] ?? "280105";
        }

        [HttpGet]
        public IActionResult VerifyWebhook(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.verify_token")] string token,
            [FromQuery(Name = "hub.challenge")] string challenge)
        {
            Console.WriteLine($"Webhook Verification: mode={mode}, token={token}, challenge={challenge}");
            _logger.LogInformation("Verification Request: mode={Mode}, token={Token}, challenge={Challenge}", mode, token, challenge);

            var configToken = _config["Facebook:VerifyToken"];
            _logger.LogInformation("Config Token: {ConfigToken}", configToken);

            if (mode == "subscribe" && token == configToken)
            {
                Console.WriteLine("Webhook verified successfully!");
                _logger.LogInformation("Verification SUCCESS");
                return Content(challenge, "text/plain");
            }

            Console.WriteLine("Webhook verification failed.");
            _logger.LogError("Verification FAILED: Mode={Mode}, Token={Token} (Expected:{ExpectedToken})", mode, token, configToken);
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveEvent()
        {
            try
            {
                Request.EnableBuffering();
                Request.Body.Position = 0;

                using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
                string rawBody = await reader.ReadToEndAsync();
                Request.Body.Position = 0;

                Console.WriteLine("Raw Request Body:\n" + rawBody);
                _logger.LogInformation("Raw Request Body:\n{RawBody}", rawBody);

                if (!VerifySignature(rawBody, Request.Headers["X-Hub-Signature-256"]))
                {
                    Console.WriteLine("Signature verification failed");
                    _logger.LogWarning("Invalid signature");
                    return Unauthorized();
                }

                using JsonDocument doc = JsonDocument.Parse(rawBody);
                JsonElement body = doc.RootElement;

                Console.WriteLine("Parsed JSON Event:\n" + JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true }));
                _logger.LogInformation("Incoming Webhook Event:\n{Json}", JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true }));

                if (body.GetProperty("object").GetString() == "page")
                {
                    foreach (var entry in body.GetProperty("entry").EnumerateArray())
                    {
                        var messagingArray = entry.GetProperty("messaging");
                        foreach (var message in messagingArray.EnumerateArray())
                        {
                            var senderId = message.GetProperty("sender").GetProperty("id").GetString();

                            // Handle referral from m.me link
                            if (message.TryGetProperty("referral", out JsonElement referralNode))
                            {
                                var refParam = referralNode.GetProperty("ref").GetString();
                                _logger.LogInformation("Referral via message: sender={SenderId}, ref={Ref}", senderId, refParam);

                                if (TryExtractUserId(refParam, out int userId))
                                {
                                    _userService.SaveFacebookPSID(userId, senderId);
                                    await _messengerService.SendReminderButtonAsync(senderId);
                                    await _messengerService.SendTextMessageAsync(senderId, "✅ Lịch nhắc nhở sẽ được gửi cho bạn sau");
                                }
                                else
                                {
                                    _logger.LogWarning("Invalid referral ref format: {RefParam}", refParam);
                                }

                                continue;
                            }

                            // Handle postback with referral (e.g., Get Started)
                            if (message.TryGetProperty("postback", out JsonElement postback) &&
                                postback.TryGetProperty("referral", out JsonElement postbackReferral))
                            {
                                var refParam = postbackReferral.GetProperty("ref").GetString();
                                _logger.LogInformation("Referral via postback: sender={SenderId}, ref={Ref}", senderId, refParam);

                                if (TryExtractUserId(refParam, out int userId))
                                {
                                    _userService.SaveFacebookPSID(userId, senderId);
                                    await _messengerService.SendReminderButtonAsync(senderId);
                                    await _messengerService.SendTextMessageAsync(senderId, "✅ Lịch nhắc nhở sẽ được gửi cho bạn sau");
                                }
                                else
                                {
                                    _logger.LogWarning("Invalid postback referral ref format: {RefParam}", refParam);
                                }

                                continue;
                            }

                            // Handle postback payload (e.g., user clicked a button)
                            else if (message.TryGetProperty("postback", out postback) &&
                                     postback.TryGetProperty("payload", out JsonElement payloadElement))
                            {
                                string payload = payloadElement.GetString();
                                _logger.LogInformation("Postback payload received from {SenderId}: {Payload}", senderId, payload);

                                if (payload == "GET_STARTED")
                                {
                                    await _messengerService.SendTextMessageAsync(senderId, "Chào mừng bạn đến với hệ thống!");
                                    await _messengerService.SendReminderButtonAsync(senderId);
                                }
                                else if (payload == "REMINDER_OPT_IN")
                                {
                                    await _messengerService.SendTextMessageAsync(senderId, "✅ Bạn đã bật nhắc nhở thuốc.");
                                }
                                else
                                {
                                    await _messengerService.SendTextMessageAsync(senderId, $"Bạn vừa chọn: {payload}");
                                }

                                continue;
                            }

                            // Handle text messages
                            if (message.TryGetProperty("message", out JsonElement msg) &&
                                msg.TryGetProperty("text", out JsonElement text))
                            {
                                string messageText = text.GetString();
                                _logger.LogInformation("Text message received from {SenderId}: {Text}", senderId, messageText);

                                if (msg.TryGetProperty("referral", out JsonElement messageReferral))
                                {
                                    // Referral from m.me?ref=...
                                    var refParam = messageReferral.GetProperty("ref").GetString();
                                    if (TryExtractUserId(refParam, out int userId))
                                    {
                                        _userService.SaveFacebookPSID(userId, senderId);
                                        await _messengerService.SendReminderButtonAsync(senderId);
                                        await _messengerService.SendTextMessageAsync(senderId, "✅ Lịch nhắc nhở sẽ được gửi cho bạn sau");
                                    }
                                    else
                                    {
                                        _logger.LogWarning("Invalid referral ref format: {RefParam}", refParam);
                                    }
                                }
                                else
                                {
                                    // No referral - normal message
                                    await _messengerService.SendTextMessageAsync(senderId, $"Ghi nhận đăng ký lịch, tải lại messenger để hoàn tất");
                                }
                            }
                        }
                    }

                    return Content("EVENT_RECEIVED", "text/plain");
                }
                else
                {
                    var objectType = body.GetProperty("object").GetString();
                    Console.WriteLine($"Unknown object type: {objectType}");
                    _logger.LogWarning("Unknown object type: {ObjectType}", objectType);
                    return Content("EVENT_RECEIVED", "text/plain");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing webhook event: {ex.Message}");
                _logger.LogError(ex, "Error processing webhook event.");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Extracts integer user ID from ref string (e.g., "user-19" => 19)
        /// </summary>
        private bool TryExtractUserId(string refParam, out int userId)
        {
            userId = 0;

            if (string.IsNullOrEmpty(refParam))
                return false;

            if (refParam.StartsWith("user-") && int.TryParse(refParam.Substring(5), out userId))
                return true;

            return int.TryParse(refParam, out userId);
        }

        private bool VerifySignature(string payload, string signatureHeader)
        {
            if (string.IsNullOrEmpty(signatureHeader))
                return false;

            try
            {
                var parts = signatureHeader.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2 || !parts[0].Equals("sha256", StringComparison.OrdinalIgnoreCase))
                    return false;

                var signature = parts[1];
                var secret = _config["Facebook:AppSecret"];

                if (string.IsNullOrEmpty(secret))
                {
                    Console.WriteLine("App secret missing in configuration");
                    return false;
                }

                using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                string computedSignature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                bool matched = computedSignature.Equals(signature, StringComparison.OrdinalIgnoreCase);

                Console.WriteLine($"Signature match: {matched}");
                return matched;
            }
            catch (Exception e)
            {
                Console.WriteLine("Signature verification exception: " + e.Message);
                return false;
            }
        }
    }
}