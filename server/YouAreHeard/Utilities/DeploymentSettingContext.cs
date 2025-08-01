using YouAreHeard.Models;

namespace YouAreHeard.Utilities
{
    public static class DeploymentSettingsContext
    {
        private static DeploymentSettings _deploymentSettings;

        public static void Initialize(DeploymentSettings settings)
        {
            _deploymentSettings = settings;
        }

        public static DeploymentSettings Settings
        {
            get
            {
                if (_deploymentSettings == null)
                    throw new InvalidOperationException("DeploymentSettingsContext is not initialized. Call Initialize() first.");
                return _deploymentSettings;
            }
        }
    }

}
