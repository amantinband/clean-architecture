using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Reminders;

public class Reminder : Entity
{
    public Guid UserId { get; }

    public DateTimeOffset DateTime { get; }

    public string Text { get; } = null!;

    public Reminder(Guid userId, string text, DateTimeOffset dateTime, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        Text = text;
        DateTime = dateTime;
        UserId = userId;
    }

    private Reminder()
    {
    }
}