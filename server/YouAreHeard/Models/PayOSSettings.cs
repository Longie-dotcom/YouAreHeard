﻿namespace YouAreHeard.Models
{
    public class PayOSSettings
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string ChecksumKey { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}