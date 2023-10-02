namespace CleanArchitecture.Domain.UnitTests.TestUtils.Reminders;

public static class ReminderConstants
{
    public const string Text = "Buy cookies";
    public static Guid Id = Guid.NewGuid();
    public static DateTimeOffset DateTime = DateTimeOffset.UtcNow.AddDays(7);
}