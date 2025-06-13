using YouAreHeard.Models;
using System;

namespace YouAreHeard.Utilities
{
    public static class ZoomSettingContext
    {
        private static ZoomSettings _zoomSettings;

        public static void Initialize(ZoomSettings settings)
        {
            _zoomSettings = settings;
        }

        public static ZoomSettings Settings
        {
            get
            {
                if (_zoomSettings == null)
                    throw new InvalidOperationException("ZoomSettingContext is not initialized. Call Initialize() first.");
                return _zoomSettings;
            }
        }
    }
}
