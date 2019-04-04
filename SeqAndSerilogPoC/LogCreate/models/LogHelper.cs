namespace LogCreate
{
    public static class LogHelper
    {
        // Fields ---------------------------------------------------------------------------------
        public const string UNKNOWN = "unknown";
        public const string APPLICATION_NAME = "ApplicationName";
        public const string APPLICATION_VERSION = "ApplicationVersion";
        public const string APPLICATION_ENVIRONMENT = "ApplicationEnvironment";
        public const string NOT_FOUND = "NOT FOUND";

        // Public Functions -----------------------------------------------------------------------
        public static string StringCheck(string input)
            => string.IsNullOrWhiteSpace(input) ? UNKNOWN : input;
    }
}
