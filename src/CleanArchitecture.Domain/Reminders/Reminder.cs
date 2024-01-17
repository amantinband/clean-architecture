using CleanArchitecture.Domain.Common;

using ErrorOr;

namespace CleanArchitecture.Domain.Reminders;

public class Reminder : CreationEntity
{
    public Guid SubscriptionId { get; }

    public string Text { get; } = null!;

    public bool IsDismissed { get; private set; }

    public Reminder(
        Guid userId,
        Guid subscriptionId,
        string text,
        DateTime dateTime,
        Guid? id = null)
            : base(id ?? Guid.NewGuid(), userId, dateTime)
    {
        SubscriptionId = subscriptionId;
        Text = text;
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