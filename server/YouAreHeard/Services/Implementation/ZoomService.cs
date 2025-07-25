﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using YouAreHeard.Enums;
using YouAreHeard.Helper;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;
using YouAreHeard.Utilities;

namespace YouAreHeard.Services.Implementation
{
    public class ZoomService : IZoomService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;

        public ZoomService(IHttpClientFactory httpClientFactory, IUserService userService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _userService = userService;
        }

        public async Task<string> GenerateZoomLink(AppointmentDTO appointment)
        {
            var token = await GetZoomAccessTokenAsync();
            var passcode = GeneratePasscode();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var startTime = appointment.ScheduleDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var body = new
            {
                topic = "Medical Appointment",
                type = 2,
                // start_time = startTime,
                duration = 30, // you can make this dynamic
                timezone = "Asia/Ho_Chi_Minh",
                password = passcode,
                settings = new
                {
                    host_video = true,
                    participant_video = true,
                    waiting_room = false
                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Map DoctorID to Zoom Email - placeholder for now
            UserDTO doctor = MapDoctorIdToZoomEmail(appointment.DoctorID);

            var response = await _httpClient.PostAsync($"https://api.zoom.us/v2/users/{ZoomSettingContext.Settings.DoctorDefaultEmail}/meetings", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create Zoom meeting. Status: {response.StatusCode}, Error: {error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);
            var zoomLink = doc.RootElement.GetProperty("join_url").GetString();

            // Get patient info
            var patient = _userService.GetUserById(appointment.PatientID);

            // Hide patient name if the appointment is anonymous
            if (appointment.IsAnonymous)
            {
                patient.Name = ConstraintWords.AnonymousName;
            }

            // Send the email
            EmailHelper.SendZoomLinkEmail(patient.Email, doctor.Name, appointment.ScheduleDate, appointment.StartTime, zoomLink, passcode);
            EmailHelper.sendZoomLinkEmailToDoctor(doctor.Email, patient.Name, appointment.ScheduleDate, appointment.StartTime, zoomLink, passcode);

            return zoomLink;
        }

        private async Task<string> GetZoomAccessTokenAsync()
        {
            var settings = ZoomSettingContext.Settings;

            var clientCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.ClientId}:{settings.ClientSecret}"));

            using var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={settings.AccountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", clientCreds);

            using var client = new HttpClient();
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve Zoom access token: {content}");
            }

            using var doc = JsonDocument.Parse(content);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

        private UserDTO MapDoctorIdToZoomEmail(int doctorId)
        {

            return _userService.GetUserById(doctorId);
        }

        private string GeneratePasscode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}