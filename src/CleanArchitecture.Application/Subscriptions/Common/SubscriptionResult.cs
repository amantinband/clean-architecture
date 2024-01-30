using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Subscriptions.Common;

public record SubscriptionResult(
    Guid Id,
    Guid UserId,
    SubscriptionType SubscriptionType)
{
    public static SubscriptionResult FromUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user.Subscription);

        return new SubscriptionResult(
            user.Subscription.Id,
            user.Id,
            user.Subscription.SubscriptionType);
    }
}