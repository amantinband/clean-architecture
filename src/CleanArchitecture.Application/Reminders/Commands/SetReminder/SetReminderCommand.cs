using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Common.Security.Request;
using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Application.Reminders.Commands.SetReminder;

[Authorize(Permissions = Permission.Reminder.Set, Policies = Policy.SelfOrAdmin)]
public class SetReminderCommand : IAuthorizeableRequest<Result<Reminder>>
{
    public Guid UserId { get; }
    public Guid SubscriptionId { get; }
    public string Text { get; }
    public DateTime DateTime { get; }

    public static Result<SetReminderCommand> TryCreate(Guid userId, Guid subscriptionId, string text, DateTime dateTime, IDateTimeProvider dateTimeProvider)
    {
        var command = new SetReminderCommand(userId, subscriptionId, text, dateTime);
        var validator = new SetReminderCommandValidator(dateTimeProvider);

        return validator.ValidateToResult(command);
    }

    private SetReminderCommand(Guid userId, Guid subscriptionId, string text, DateTime dateTime)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        Text = text;
        DateTime = dateTime;
    }
}