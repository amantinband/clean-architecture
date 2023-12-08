using CleanArchitecture.Domain.Common;

using ErrorOr;

namespace CleanArchitecture.Domain.Reminders;

public class Reminder : Entity
{
    public Guid SubscriptionId { get; }

    public DateTimeOffset DateTime { get; }

    public DateOnly Date => DateOnly.FromDateTime(DateTime.Date);

    public string Text { get; } = null!;

    public bool IsDismissed { get; private set; }

    public Reminder(Guid subscriptionId, string text, DateTimeOffset dateTime, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        Text = text;
        DateTime = dateTime;
        SubscriptionId = subscriptionId;
    }

    public ErrorOr<Success> Dismiss()
    {
        if (IsDismissed)
        {
            return Error.Conflict(description: "Reminder already dismissed");
        }

        IsDismissed = true;

        return Result.Success;
    }

    private Reminder()
    {
    }
}