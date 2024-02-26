using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.Reminders;

public class Reminder : Entity<ReminderId>
{
    public UserId UserId { get; }

    public Guid SubscriptionId { get; }

    public DateTime DateTime { get; }

    public DateOnly Date => DateOnly.FromDateTime(DateTime.Date);

    public string Text { get; } = null!;

    public bool IsDismissed { get; private set; }

    public Reminder(
        UserId userId,
        Guid subscriptionId,
        string text,
        DateTime dateTime,
        ReminderId? id = null)
            : base(id ?? ReminderId.NewUnique())
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        Text = text;
        DateTime = dateTime;
    }

    public static Result<Reminder> TryCreate(Guid userId, Guid subscriptionId, string text, DateTime dateTime, ReminderId? id = null)
    {
        return UserId.TryCreate(userId)
            .Map(userId => new Reminder(userId, subscriptionId, text, dateTime, id));
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
}