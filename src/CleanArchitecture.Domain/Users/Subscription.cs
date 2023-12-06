using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.Subscriptions;

public class Subscription : Entity
{
    public SubscriptionType SubscriptionType { get; } = null!;

    public Subscription(SubscriptionType subscriptionType, Guid? id = null)
            : base(id ?? Guid.NewGuid())
    {
        SubscriptionType = subscriptionType;
    }

    private Subscription()
    {
    }
}