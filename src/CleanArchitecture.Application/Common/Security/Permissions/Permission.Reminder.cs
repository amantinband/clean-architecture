namespace CleanArchitecture.Application.Common.Security.Permissions;

public static partial class Permission
{
    public static class Reminder
    {
        public const string Set = "set:reminder";
        public const string Get = "get:reminder";
        public const string Dismiss = "dismiss:reminder";
        public const string Delete = "delete:reminder";
    }
}