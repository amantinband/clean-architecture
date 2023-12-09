using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Policies;
using CleanArchitecture.Domain.Reminders;

using ErrorOr;

namespace CleanArchitecture.Application.Reminders.Commands.SetReminder;

[Authorize(Permissions = Permission.Reminder.Set, Policies = Policy.SelfOrAdmin)]
public record SetReminderCommand(Guid UserId, Guid SubscriptionId, string Text, DateTime DateTime)
    : IAuthorizeableRequest<ErrorOr<Reminder>>;