namespace YouAreHeard.Models
{
    public class PayOSWebhookDTO
    {
        public string OrderCode { get; set; }  // e.g., "APPT_123"
        public int Amount { get; set; }        // e.g., 100000 VND
        public string Status { get; set; }     // e.g., "PAID"
        public string Description { get; set; } // Optional
        public DateTime PaymentTime { get; set; } // Optional
        public string TransactionId { get; set; } // Optional
    }
}