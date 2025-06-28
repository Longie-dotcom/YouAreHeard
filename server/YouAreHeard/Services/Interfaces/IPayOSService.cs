using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IPayOSService
    {
        string GeneratePaymentUrl(PayOSPaymentRequest request);
    }
}