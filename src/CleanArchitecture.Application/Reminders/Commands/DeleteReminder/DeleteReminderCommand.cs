using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Application.Common.Security.Request;

namespace CleanArchitecture.Application.Reminders.Commands.DeleteReminder;

[Authorize(Permissions = Permission.Reminder.Delete, Policies = Policy.SelfOrAdmin)]
public record DeleteReminderCommand(Guid UserId, Guid SubscriptionId, Guid ReminderId)
    : IAuthorizeableRequest<Result<Unit>>;