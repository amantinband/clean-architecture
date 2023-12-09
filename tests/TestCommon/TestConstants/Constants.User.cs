using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;

namespace TestCommon.TestConstants;

public static partial class Constants
{
    public static class User
    {
        public const string FirstName = "Amiko";
        public const string LastName = "Mantinband";
        public const string Email = "amiko@mantinband.com";
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly List<string> Permissions =
        [
            Permission.Reminder.Get,
            Permission.Reminder.Set,
            Permission.Reminder.Delete,
            Permission.Reminder.Dismiss,
            Permission.Subscription.Create,
            Permission.Subscription.Delete,
        ];

        public static readonly List<string> Roles =
        [
            Role.Admin
        ];
    }
}