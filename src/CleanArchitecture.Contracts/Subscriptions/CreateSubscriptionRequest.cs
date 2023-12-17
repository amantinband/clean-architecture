using CleanArchitecture.Contracts.Common;

namespace CleanArchitecture.Contracts.Subscriptions;

public record CreateSubscriptionRequest(
    string FirstName,
    string LastName,
    string Email,
    SubscriptionType SubscriptionType);