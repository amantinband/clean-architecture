using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Subscriptions.Common;

public record SubscriptionResult(
    Guid Id,
    Guid UserId,
    string UserFirstName,
    string UserLastName,
    SubscriptionType SubscriptionType)
{
    public static SubscriptionResult FromUser(User user)
    {
        return new SubscriptionResult(
            user.Subscription.Id,
            user.Id,
            user.FirstName,
            user.LastName,
            user.Subscription.SubscriptionType);
    }
}