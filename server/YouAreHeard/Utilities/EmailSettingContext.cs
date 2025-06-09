using YouAreHeard.Models;

public static class EmailSettingsContext
{
    private static EmailSettings _emailSettings;

    public static void Initialize(EmailSettings settings)
    {
        _emailSettings = settings;
    }

    public static EmailSettings Settings
    {
        get
        {
            if (_emailSettings == null)
                throw new InvalidOperationException("EmailSettingsContext is not initialized. Call Initialize() first.");
            return _emailSettings;
        }
    }
}

