using ErrorOr;

namespace CleanArchitecture.Domain.Users;

public static class UserErrors
{
    public static Error CannotCreateMoreRemindersThanPlanAllows { get; } = Error.Validation(
        code: "UserErrors.CannotCreateMoreRemindersThanPlanAllows",
        description: "Cannot create more reminders than plan allows");
}