using YouAreHeard.Models;

namespace YouAreHeard.Utilities
{
    public static class FirebaseSettingsContext
    {
        private static FirebaseSettings _firebaseSettings;

        public static void Initialize(FirebaseSettings settings)
        {
            _firebaseSettings = settings;
        }

        public static FirebaseSettings Settings
        {
            get
            {
                if (_firebaseSettings == null)
                    throw new InvalidOperationException("FirebaseSettingsContext is not initialized. Call Initialize() first.");
                return _firebaseSettings;
            }
        }
    }
}