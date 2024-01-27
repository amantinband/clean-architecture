namespace CleanArchitecture.Domain.Users;

public static class UserErrors
{
    public static Error CannotCreateMoreRemindersThanSubscriptionAllows { get; } = Error.Validation(
        code: "UserErrors.CannotCreateMoreRemindersThanSubscriptionAllows",
        message: "Cannot create more reminders than subscription allows");
}