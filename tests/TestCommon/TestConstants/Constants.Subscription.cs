using CleanArchitecture.Domain.Users;

namespace TestCommon.TestConstants;

public static partial class Constants
{
    public static class Subscription
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly SubscriptionType Type = SubscriptionType.Basic;
    }
}