using YouAreHeard.Models;

namespace YouAreHeard.Utilities
{
    public static class PayOSSettingContext
    {
        private static PayOSSettings _settings;

        public static void Initialize(PayOSSettings settings)
        {
            _settings = settings;
        }

        public static PayOSSettings Settings
        {
            get
            {
                if (_settings == null)
                    throw new InvalidOperationException("PayOSSettingContext is not initialized. Call Initialize() first.");
                return _settings;
            }
        }
    }
}
