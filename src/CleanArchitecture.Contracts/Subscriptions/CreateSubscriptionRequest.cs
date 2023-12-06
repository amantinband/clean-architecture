using CleanArchitecture.Contracts.Common;

namespace CleanArchitecture.Contracts.Subscriptions;

public record CreateSubscriptionRequest(SubscriptionType? SubscriptionType);