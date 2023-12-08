using CleanArchitecture.Contracts.Common;

namespace CleanArchitecture.Contracts.Subscriptions;

public record SubscriptionResponse(
    Guid Id,
    Guid UserId,
    SubscriptionType SubscriptionType);