namespace TestCommon.TestConstants;

public static partial class Constants
{
    public static class Reminder
    {
        public const string Text = "Remind to to dismiss this reminder";
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly DateTime DateTime = DateTime.UtcNow
            .AddDays(1).Date
            .AddHours(8); // tomorrow 8 AM
    }
}