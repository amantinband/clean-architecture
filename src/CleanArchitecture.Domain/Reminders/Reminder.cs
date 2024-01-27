using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Reminders;

public class Reminder : Entity
{
    public Guid UserId { get; }

    public Guid SubscriptionId { get; }

    public DateTime DateTime { get; }

    public DateOnly Date => DateOnly.FromDateTime(DateTime.Date);

    public string Text { get; } = null!;

    public bool IsDismissed { get; private set; }

    public Reminder(
        Guid userId,
        Guid subscriptionId,
        string text,
        DateTime dateTime,
        Guid? id = null)
            : base(id ?? Guid.NewGuid())
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        Text = text;
        DateTime = dateTime;
    }

    public Result<Unit> Dismiss()
    {
        if (IsDismissed)
        {
            return Error.Conflict("Reminder already dismissed");
        }

        IsDismissed = true;

        return Result.Success();
    }

    private Reminder()
    {
    }
}